using System.Collections.Generic;
using UnityEngine;

namespace RandomMapGenerator
{
    public static class WallGenerator{
    
        public static void CreateWalls(HashSet<Vector2> floorPositions, TilemapVisualizer tilemapVisualizer)
        {
            HashSet<Vector2> basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.CardinalDirectionsList);
            HashSet<Vector2> cornerWallPositions = FindWallsInDirections(floorPositions, Direction2D.DiagonalDirectionsList);
            // foreach (var position in basicWallPositions)
            // {
            //     tilemapVisualizer.PaintSingleBasicWall(position);
            // }

            CreateBasicWalls(tilemapVisualizer, basicWallPositions, floorPositions);
            CreateCornerWalls(tilemapVisualizer, cornerWallPositions, floorPositions);

            basicWallPositions.UnionWith(cornerWallPositions);
            //return basicWallPositions;
        }

        private static void CreateBasicWalls(TilemapVisualizer tilemapVisualizer, HashSet<Vector2> basicWallPositions, 
            HashSet<Vector2> floorPositions)
        {
            foreach (var position in basicWallPositions)
            {
                string neighboursBinaryType = "";
                foreach (var direction in Direction2D.CardinalDirectionsList)
                {
                    var neighbourPosition = position + direction;
                    if (floorPositions.Contains(neighbourPosition))
                        neighboursBinaryType += '1';
                    else
                    {
                        neighboursBinaryType += '0';
                    }
                }
                tilemapVisualizer.PaintSingleBasicWall(position, neighboursBinaryType);
            }
        }

        public static void CreateCornerWalls(TilemapVisualizer tilemapVisualizer, HashSet<Vector2> cornerWallPosition,
            HashSet<Vector2> floorPosition)
        {
            foreach (var position in cornerWallPosition)
            {
                string neighbourBinaryType = "";

                foreach (var direction in Direction2D.EightDirectionsList)
                {
                    var neighbourPosition = position + direction;
                    if (floorPosition.Contains(neighbourPosition))
                    {
                        neighbourBinaryType += '1';
                    }
                    else
                    {
                        neighbourBinaryType += '0';
                    }
                }
            
                tilemapVisualizer.PaintSingleCornerWall(position, neighbourBinaryType);
            }
        }

        public static  HashSet<Vector2> FindWallsInDirections(HashSet<Vector2> floorPositions, List<Vector2> directionList)
        {
            HashSet<Vector2> wallPositions = new HashSet<Vector2>();

            foreach (var position in floorPositions)
            {
                foreach (var direction in directionList)
                {
                    var neighbourPosition = position + direction;

                    if (floorPositions.Contains(neighbourPosition) == false)
                    {
                        wallPositions.Add(neighbourPosition);
                    
                    }
                }
            }


            return wallPositions;
        }
    
        // public static void CreateWalls(HashSet<Vector2> floorPositions, TilemapVisualizer tilemapVisualizer)
        // {
        //     var basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.CardinalDirectionsList);
        //     var cornerWallPositions = FindWallsInDirections(floorPositions, Direction2D.DiagonalDirectionsList);
        //     // foreach (var position in basicWallPositions)
        //     // {
        //     //     tilemapVisualizer.PaintSingleBasicWall(position);
        //     // }
        //
        //     for (int i = 0; i < ((HashSet<Vector2>)basicWallPositions[0]).Count; i++)
        //     {
        //         tilemapVisualizer.PaintSingleBasicWall(((HashSet<Vector2>)basicWallPositions[0]).ElementAt(i), 
        //             ((List<KeyValuePair<string,Vector2>>)basicWallPositions[1]).ElementAt(i).Key);
        //     }
        // }
        //
        // public static object[] FindWallsInDirections(HashSet<Vector2> floorPositions, List<KeyValuePair<string,Vector2>> directionList)
        // {
        //     HashSet<Vector2> wallPositions = new HashSet<Vector2>();
        //     List<KeyValuePair<string,Vector2>> wallDirection = new List<KeyValuePair<string,Vector2>>();
        //    
        //     foreach (var position in floorPositions)
        //     {
        //         foreach (var direction in directionList)
        //         {
        //             var neighbourPosition = position + direction.Value;
        //
        //             if (floorPositions.Contains(neighbourPosition) == false)
        //             {
        //                 wallPositions.Add(neighbourPosition);
        //                 wallDirection.Add(direction);
        //             }
        //         }
        //     }
        //
        //     object[] wallInfo = { wallPositions, wallDirection };
        //     return wallInfo;
        // }
    }
}
