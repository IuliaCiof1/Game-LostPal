using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace RandomMapGenerator
{
   public class SimpleRandomMapGenerator : MonoBehaviour
   {
      [SerializeField] private Vector2 startPosition = Vector2.zero;
      [SerializeField] private int iterations = 10; //used for more randomness
      [SerializeField] private int walkLength = 10; //control the size of the path
      [SerializeField] public bool startRandomlyEachIteration = true;
      [SerializeField] private float width, height;
      [SerializeField] private TilemapVisualizer tilemapVisualizer;
      [SerializeField] private GameObject coinPrefab, playerPrefab;
      [SerializeField] private GameObject[] treePrefab;
      [SerializeField] private int minNumberOfObstacles=1, maxNumberOfObstacles = 10, minNumberOfTrees=1, maxNumberOfTrees = 10;
      [SerializeField] private Vector2 offset=Vector2.zero;
      [FormerlySerializedAs("distanceBetweenTrees")] [SerializeField] private float minDistanceTrees;
      [SerializeField] private float minDistancePlayerCoin;
      
      public void Awake() //Awake() always executes before Start()
      {
         //Endpoints
         float[] widthPos = { startPosition.x, startPosition.x + width };
         float[] heightPos = { startPosition.y - height / 2f, startPosition.y + height / 2f };
         
         HashSet<Vector2> floorPositions = RunRandomWalk(widthPos, heightPos);
         tilemapVisualizer.PaintFloorTiles(floorPositions);
         
         WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
         WaterGenerator.CreateWater(floorPositions, tilemapVisualizer, widthPos, heightPos);
         
         //spawn obstacle
         tilemapVisualizer.PaintObstacles(minNumberOfObstacles, maxNumberOfObstacles, floorPositions);
         //spawn trees
         InstantiateTrees(floorPositions);
         
         //spawn player
         GameObject player = Instantiate(playerPrefab, GetRandomFloorPosition(floorPositions) + offset,
                    Quaternion.identity);

         for (int i = 0; i < 10; i++)
         {
            Vector2 pos = GetRandomFloorPosition(floorPositions) + offset;
            if (Vector2.Distance(player.transform.position, pos) > minDistancePlayerCoin )
            {
               //spawn coin
               Instantiate(coinPrefab, pos, Quaternion.identity);
               break;
            }
         }
         

      }

      public void InstantiateTrees(HashSet<Vector2> floorPositions)
      {
         GameObject trees = Instantiate(new GameObject("Trees"));
         List<GameObject> treesList = new List<GameObject>();
         int randomNr = Random.Range(minNumberOfTrees, maxNumberOfTrees);
         for (int i = 0; i <= randomNr; i++)
         {
            Vector2 pos = GetRandomFloorPosition(floorPositions);

            //only spawn tree if it's not too close to any other tree
            if (treesList.TrueForAll(treeGameObject =>
                   Vector2.Distance(treeGameObject.transform.position, pos) > minDistanceTrees))
            {
               GameObject tree = Instantiate(treePrefab[Random.Range(0, treePrefab.Length)], pos, Quaternion.identity,
                  trees.transform);
               treesList.Add(tree);
               floorPositions.Remove(pos);
            }
           
         }
      }
      public Vector2 GetRandomFloorPosition(HashSet<Vector2> floorPositions)
      {
         Vector2 pos = Vector2.zero;

         
            pos = floorPositions.ElementAt(Random.Range(0, floorPositions.Count - 1));
            
               return pos;
      } 
   
      public HashSet<Vector2> RunRandomWalk(float[] widthPos, float[] heightPos)
      {
         var currentPosition = startPosition;
         HashSet<Vector2> floorPositions = new HashSet<Vector2>();

         // //Endpoints
         // float[] widthPos = { startPosition.x, startPosition.x + width };
         // float[] heightPos = { startPosition.y - height / 2f, startPosition.y + height / 2f };
      
         for (int i = 0; i < iterations; i++)
         {
            //create a path
            var path = ProceduralGenerationAlgorithm.SimpleRandomWalk(currentPosition, walkLength, widthPos, heightPos);
        
            floorPositions.UnionWith(path); //copy path positions to floorPositions hash set
         
            if (startRandomlyEachIteration)
            {
               //at each iteration continue path from a random location of the already generated path
               currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }

         }

         return floorPositions;
      }
   }
}
