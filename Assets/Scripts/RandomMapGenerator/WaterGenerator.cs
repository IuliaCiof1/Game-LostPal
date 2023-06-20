using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RandomMapGenerator
{
    public static class WaterGenerator{

        public static void CreateWater(HashSet<Vector2> floorPositions, TilemapVisualizer tilemapVisualizer, float[] widthPos, float[] heightPos)
        {
            HashSet<Vector2> avoidPosition = new HashSet<Vector2>(floorPositions);  //copy the values, don't just assign or change the original hash set
            HashSet<Vector2> waterPositions = FindWaterPosition(avoidPosition, Direction2D.EightDirectionsList, widthPos, heightPos);
           // int i=0;
            while (waterPositions.Count>0)
            {
                // foreach (var position in waterPositions)
                // {
                //     tilemapVisualizer.PaintSingleWater(position);
                // }
                //
                tilemapVisualizer.PaintSingleWater(waterPositions);
                avoidPosition.UnionWith(waterPositions);
                waterPositions=FindWaterPosition(avoidPosition, Direction2D.EightDirectionsList, widthPos, heightPos);
                //Debug.Log("positions: " + waterPositions.Count);
                //i++;
            }
            
        }
        
        public static HashSet<Vector2> FindWaterPosition(HashSet<Vector2> avoidPositions, List<Vector2> directionList, float[] widthPos, float[] heightPos)
        {
            HashSet<Vector2> waterPosition = new HashSet<Vector2>();
            
            foreach (var position in avoidPositions)
            {
                foreach (var direction in directionList)
                {
                    var neighbourPosition = position + direction;
                    if (!avoidPositions.Contains(neighbourPosition))
                    {
                        if (position.x >= widthPos[0] &&
                            position.x <= widthPos[1] && //check if the new position is withing the map limit
                            position.y >= heightPos[0] && position.y <= heightPos[1])
                        {
                            //Debug.Log("aaaaaaaaaa");
                            waterPosition.Add(neighbourPosition);
                        }
                    }
                }
            }
//            Debug.Log("aici positions: " + waterPosition.ElementAt(0));
            return waterPosition;
        }
        
        
//         public static void CreateWater(HashSet<Vector2> wallPositions, TilemapVisualizer tilemapVisualizer, float[] widthPos, float[] heightPos)
//         {
//             
//             HashSet<Vector2> waterPositions = FindWaterPosition(wallPositions, new HashSet<Vector2>() , Direction2D.EightDirectionsList, widthPos, heightPos);
//             int i=0;
//             while (waterPositions.Count>0)
//             {
//                 foreach (var position in waterPositions)
//                 {
//                     tilemapVisualizer.PaintSingleWater(position);
//                 }
//
//                 wallPositions.UnionWith(waterPositions);
//                 waterPositions=FindWaterPosition(wallPositions, waterPositions, Direction2D.EightDirectionsList, widthPos, heightPos);
//                 Debug.Log("positions: " + waterPositions.Count);
//                 i++;
//             }
//             
//         }
//         
//         public static HashSet<Vector2> FindWaterPosition(HashSet<Vector2> wallPositions, HashSet<Vector2> waterPositions, List<Vector2> directionList, float[] widthPos, float[] heightPos)
//         {
//             HashSet<Vector2> waterPosition = new HashSet<Vector2>();
//             
//             foreach (var position in wallPositions)
//             {
//                 foreach (var direction in directionList)
//                 {
//                     var neighbourPosition = position + direction;
//                     if (!wallPositions.Contains(neighbourPosition)&&!waterPositions.Contains(neighbourPosition))
//                     {
//                         if (position.x >= widthPos[0] &&
//                             position.x <= widthPos[1] && //check if the new position is withing the map limit
//                             position.y >= heightPos[0] && position.y <= heightPos[1])
//                         {
//                             Debug.Log("aaaaaaaaaa");
//                             waterPosition.Add(neighbourPosition);
//                         }
//                     }
//                 }
//             }
// //            Debug.Log("aici positions: " + waterPosition.ElementAt(0));
//             return waterPosition;
//         }
    }
}