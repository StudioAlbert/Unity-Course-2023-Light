using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PCGDungeon
{
    public class PCGDungeon_GeneratorWalls : MonoBehaviour
    {

        [SerializeField] private TileBase _upWallTile;
        [SerializeField] private TileBase _downWallTile;
        [SerializeField] private TileBase _leftWallTile;
        [SerializeField] private TileBase _rightWallTile;

        public HashSet<Vector2Int> GetWalls(HashSet<Vector2Int> roomPositions, List<Vector2Int> availablesDirections)
        {
            // All wall positions
            HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();

            foreach (Vector2Int roomPosition in roomPositions)
            {
                foreach (Vector2Int direction in availablesDirections)
                {
                    Vector2Int wallPosition = roomPosition + direction;

                    // Test if the new position is already knwon as room positions
                    // If not it is a wall
                    if (!roomPositions.Contains(wallPosition))
                    {
                        if (!wallPositions.Contains(wallPosition))
                        {
                            wallPositions.Add(wallPosition);
                        }
                    }
                    
                    
                }
            }

            return wallPositions;
        }
        
    }
}