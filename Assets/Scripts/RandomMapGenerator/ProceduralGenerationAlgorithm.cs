using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RandomMapGenerator
{
    public static class ProceduralGenerationAlgorithm
    {
        public static HashSet<Vector2> SimpleRandomWalk(Vector2 startPosition, int walkLength, float []widthPos, float[] heightPos)
        {
            HashSet<Vector2> path = new HashSet<Vector2>(); //connected random positions

            path.Add(startPosition);
            var previousPosition = startPosition;
            var newPosition = previousPosition + Direction2D.GetRandomCardinalDirection(); //get a position near the previous position in a random direction

            for (int i = 0; i < walkLength; i++)
            {
                if (newPosition.x >= widthPos[0] && newPosition.x <= widthPos[1] && //check if the new position is withing the map limit
                    newPosition.y >= heightPos[0] && newPosition.y <= heightPos[1])
                {
                    //Debug.Log("newpos = "+newPosition);
                    path.Add(newPosition);
                    previousPosition = newPosition;
                    newPosition = previousPosition + Direction2D.GetRandomCardinalDirection(); 
                }
                else //if the new position is not within the given width and height of the map, find a new position from a random position of the already created path
                {
                    newPosition = path.ElementAt(Random.Range(0, path.Count));
                }
            
            }
       
            return path;
        }
    }


    public static class Direction2D
    {
        public static List<Vector2> CardinalDirectionsList = new List<Vector2>
        {
            new Vector2(0, 0.33f), //up
            new Vector2(0.33f, 0), //right
            new Vector2(0, -0.33f), //down
            new Vector2(-0.33f, 0) //left
        };
    
        public static List<Vector2> DiagonalDirectionsList = new List<Vector2>
        {
            new Vector2(0.33f, 0.33f), //up-right
            new Vector2(0.33f, -0.33f), //right-down
            new Vector2(-0.33f, -0.33f), //down-left
            new Vector2(-0.33f, 0.33f) //left-up
        };
        public static List<Vector2> EightDirectionsList = new List<Vector2>
        {
            new Vector2(0, 0.33f), //up
            new Vector2(0.33f, 0.33f), //up-right
            new Vector2(0.33f, 0), //right
            new Vector2(0.33f, -0.33f), //right-down
            new Vector2(0, -0.33f), //down
            new Vector2(-0.33f, -0.33f), //down-left
            new Vector2(-0.33f, 0), //left
            new Vector2(-0.33f, 0.33f) //left-up

        };
        //dictionary cant hold duplicate items
        public static List<KeyValuePair<string,Vector2>> CardinalWallDirectionsList = new  List<KeyValuePair<string,Vector2>>
        {
            new KeyValuePair<string, Vector2>("up", new Vector2(0, 0.33f)), //up
            new KeyValuePair<string, Vector2>("right", new Vector2(0.33f, 0)), //right
            new ("down", new Vector2(0, -0.33f)), //down
            new ("left", new Vector2(-0.33f, 0)) //left
        };

        public static Vector2 GetRandomCardinalDirection()
        {
            return CardinalDirectionsList[Random.Range(0, CardinalDirectionsList.Count)];
        }
    }
}