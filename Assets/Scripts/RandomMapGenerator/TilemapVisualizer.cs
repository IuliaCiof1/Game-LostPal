using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace RandomMapGenerator
{
   public class TilemapVisualizer : MonoBehaviour
   {
      [SerializeField] private Tilemap floorTilemap;
      [SerializeField] private Tilemap wallTilemap, waterTilemap, obstacleTilemap;
      [SerializeField] private TileBase[] floorTiles, obstacleTiles;
      [SerializeField] private TileBase wallTiles;

      [SerializeField] private TileBase wallFull,
         wallTop,
         wallRight,
         wallBottom,
         wallLeft,
         wallInnerCornerUpLeft,
         wallInnerCornerUpRight,
         wallInnerCornerDownLeft,
         wallInnerCornerDownRight,
         wallDiagonalCornerDownRight,
         wallDiagonalCornerDownLeft,
         wallDiagonalCornerUpRight,
         wallDiagonalCornerUpLeft,
         water
         ;
      public void PaintFloorTiles(IEnumerable<Vector2> floorPositions)
      {
         PaintTilesRandom(floorPositions, floorTilemap, floorTiles);
      }

      public void PaintTilesRandom(IEnumerable<Vector2>positions, Tilemap tilemap, TileBase[] tiles)
      {
         foreach (var position in positions)
         {
         
            PaintSingleTile(tilemap, tiles[Random.Range(0, tiles.Length-1)], position);
         }
      }
      
      public void PaintTiles(IEnumerable<Vector2>positions, Tilemap tilemap, TileBase tile)
      {
         foreach (var position in positions)
         {
         
            PaintSingleTile(tilemap, tile, position);
         }
         
         //tilemap.GetComponent<TilemapCollider2D>().ProcessTilemapChanges(); //refresh tilemapcollider
         TilemapCollider2D col = tilemap.GetComponent<TilemapCollider2D>();

         col.enabled = false;
         col.enabled = true;
      }

      public void PaintObstacles(int minRange, int maxRange, HashSet<Vector2> positions)
      {
         PaintRandomTiles(minRange, maxRange, obstacleTilemap, obstacleTiles, positions);
      }
      
      public void PaintRandomTiles(int minRange, int maxRange, Tilemap tilemap, TileBase[] tiles, HashSet<Vector2>positions)
      {
         int range = Random.Range(minRange, maxRange);
        for(int i=0; i<range; i++)
        {
           Vector2 randomPosition = positions.ElementAt(Random.Range(0, positions.Count() - 1));
           positions.Remove(randomPosition);
            PaintSingleTile(tilemap, tiles[Random.Range(0, tiles.Length-1)], randomPosition);
        }
      }
      
      public void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2 position)
      {
         //StartCoroutine(PaintSingleTileSlow(tilemap, tile, position));
         var tilePosition = tilemap.WorldToCell((Vector3)position);
         tilemap.SetTile(tilePosition, tile);
      }

      // public IEnumerator PaintSingleTileSlow(Tilemap tilemap, TileBase tile, Vector2 position)
      // {
      //    var tilePosition = tilemap.WorldToCell((Vector3)position);
      //    tilemap.SetTile(tilePosition, tile);
      //    yield return new WaitForSeconds(10);
      // }
      public void PaintSingleCornerWall(Vector2 position, string binaryType)
      {
         int typeAsInt = Convert.ToInt32(binaryType, 2);
         TileBase tile = null;

         if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeAsInt))
            tile = wallInnerCornerDownLeft;
         else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeAsInt))
            tile = wallInnerCornerDownRight;
         else if (WallTypesHelper.wallInnerCornerUpLeft.Contains(typeAsInt))
            tile = wallInnerCornerUpLeft;
         else if (WallTypesHelper.wallInnerCornerUpRight.Contains(typeAsInt))
            tile = wallInnerCornerUpRight;
         else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeAsInt))
            tile = wallDiagonalCornerDownLeft;
         else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeAsInt))
            tile = wallDiagonalCornerDownRight;
         else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeAsInt))
            tile = wallDiagonalCornerUpLeft;
         else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeAsInt))
            tile = wallDiagonalCornerUpRight;
         else if (WallTypesHelper.wallFullEightDirections.Contains(typeAsInt))
            tile = wallFull;
         else if (WallTypesHelper.wallBottmEightDirections.Contains(typeAsInt))
            tile = wallBottom;
      
         if(tile!=null)
            PaintSingleTile(wallTilemap, tile, position);
      }
   
      public void PaintSingleBasicWall(Vector2 position, string binaryType)
      {
     
         int typeAsInt = Convert.ToInt32(binaryType, 2);
         TileBase tile = null;

         if (WallTypesHelper.wallTop.Contains(typeAsInt))
            tile = wallTop;
         else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
            tile = wallRight;
         else if (WallTypesHelper.wallBottom.Contains(typeAsInt))
            tile = wallBottom;
         else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
            tile = wallLeft;
         else if (WallTypesHelper.wallFull.Contains(typeAsInt))
            tile = wallFull;
      
         if(tile!=null)
            PaintSingleTile(wallTilemap, tile, position);
      }

      public void PaintSingleWater(IEnumerable<Vector2> position)
      {
         PaintTiles(position, waterTilemap, water);
      }

      
      
      
   }
}
