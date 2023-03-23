using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace PCGDungeon
{
    [ExecuteInEditMode]
    public class PCGDungeon_MapPainter : MonoBehaviour
    {
        [Header("Maps")]
        [SerializeField] private Tilemap _floorMap;
        [SerializeField] private Tilemap _wallMap;
        [SerializeField] private Tilemap _patchMap;
        
        [Header("Tiles")]
        [SerializeField] private RuleTile _ruledTile;
        [SerializeField] private TileBase _defaultTile;
        [SerializeField] private TileBase _floorTile;
        [SerializeField] private TileBase _upWallTile;
        [SerializeField] private TileBase _downWallTile;
        [SerializeField] private TileBase _leftWallTile;
        [SerializeField] private TileBase _rightWallTile;
        [SerializeField] private TileBase _upRightWallTile;
        [SerializeField] private TileBase _downRightWallTile;
        [SerializeField] private TileBase _downLeftWallTile;
        [SerializeField] private TileBase _upLeftWallTile;        
        [SerializeField] private TileBase _innerDownRightWallTile;
        [SerializeField] private TileBase _innerDownLeftWallTile;

        [SerializeField] private TileBase _fullMapTile;
        
        [SerializeField] private List<TileBase> _gradient;

        [Header("Generators")]
        [SerializeField] private PCGDungeon_GeneratorRandomWalk _rndWalkGenerator;
        [SerializeField] private PCGDungeon_GeneratorTreeGraph _treeGraphGenerator;
        [SerializeField] private PCGDungeon_GeneratorBSPAreaGuaranteed _bspAreaGenerator;
        [SerializeField] private Generator_GameOfLife _gameOfLifeGenerator;
        [SerializeField] private PCGDungeon_GeneratorWalls _generatorWalls;
        
        private HashSet<Vector2Int> floorPositions;

        public void RandomWalkGenerate()
        {
            // HashSet<Vector2Int> floorPositions;

            // Create the floor --
            _floorMap.ClearAllTiles();
            _patchMap.ClearAllTiles();
            
            floorPositions = _rndWalkGenerator.GenerateSimpleRoom();
            
            // Get the walls and eventually patch the floor
            HashSet<Vector2Int> basicWalls = _generatorWalls.GetWalls(floorPositions, Neighbourhood.Cardinals);
            HashSet<Vector2Int> patchesA = _rndWalkGenerator.PatchSurroundedTiles(basicWalls, floorPositions, Neighbourhood.Cardinals, 0.5f);
            HashSet<Vector2Int> patchesB = _rndWalkGenerator.PatchSurroundedTiles(basicWalls, floorPositions, Neighbourhood.Cardinals, 0.5f);
            patchesA.UnionWith(patchesB);

            MapPainter.MapPaint(floorPositions, _floorMap, _ruledTile);
            MapPainter.MapPaint(patchesA, _patchMap, _ruledTile);
            
            // PaintWalls(floorPositions);
            //PaintWalls();
            
        }
        
        public void CorridorsFirstGenerate()
        {
            // HashSet<Vector2Int> floorPositions;

            // Create the floor --
            _floorMap.ClearAllTiles();
            _patchMap.ClearAllTiles();
            
            floorPositions = _rndWalkGenerator.GenerateWithCorridors();

            MapPainter.MapPaint(floorPositions, _floorMap, _floorTile);
            
            // PaintWalls(floorPositions);
            PaintWalls();
            
        }
        
        public void TreeGraphGenerate()
        {
            // HashSet<Vector2Int> floorPositions;

            // Create the floor --
            _floorMap.ClearAllTiles();
            floorPositions = _treeGraphGenerator.Generate();
            MapPainter.MapPaint(floorPositions, _floorMap, _floorTile);
            
            // PaintWalls(floorPositions);
            PaintWalls();
        }
        
        public void BSPAreaGenerate()
        {
            // HashSet<Vector2Int> floorPositions;

            // Create the floor --
            _floorMap.ClearAllTiles();
            floorPositions = _bspAreaGenerator.Generate();
            MapPainter.MapPaint(floorPositions, _floorMap, _floorTile);
            
            // PaintWalls(floorPositions);
            PaintWalls();
        }

        public void GameOfLifeGenerate()
        {
            _floorMap.ClearAllTiles();
            _wallMap.ClearAllTiles();
            _patchMap.ClearAllTiles();
            
            floorPositions = _gameOfLifeGenerator.Generate();
            MapPainter.MapPaint(floorPositions, _floorMap, _floorTile);
            //
            // HashSet<Vector2Int> cavePositions = _gameOfLifeGenerator.GetCave();
            // MapPainter.MapPaint(cavePositions, _wallMap, _defaultTile);
            //
            // var floodFills = _gameOfLifeGenerator.GetFloodFills();
            // int gradientIndex = 0;
            // foreach (var floodFill in floodFills)
            // {
            //     MapPainter.MapPaint(floodFill, _patchMap, MapPainter.PickGradientColor(_gradient, gradientIndex++));
            // }
            
        }
        
        public void GameOfLifeIterate()
        {
            _wallMap.ClearAllTiles();
            floorPositions = _gameOfLifeGenerator.GameOfLifeIterate();
            MapPainter.MapPaint(floorPositions, _wallMap, _floorTile);

        }
        
        public void GameOfLifeReset()
        {
            _wallMap.ClearAllTiles();
            floorPositions = _gameOfLifeGenerator.Initiate();
            MapPainter.MapPaint(floorPositions, _wallMap, _floorTile);
            
        }
        // public void PaintWalls(HashSet<Vector2Int> floorPositions)
        public void PaintWalls()
        {
            // Create the walls
            _wallMap.ClearAllTiles();

            HashSet<Vector2Int> basicWalls = _generatorWalls.GetWalls(floorPositions, Neighbourhood.Cardinals);
            foreach (Vector2Int wall in basicWalls)
            {
                //CellPaint(_wallMap, _defaultTile, wall);
                BasicWallPaint(_wallMap, wall, Neighbourhood.GetHash(wall, floorPositions, Neighbourhood.Cardinals));
            }
            
            HashSet<Vector2Int> cornerWalls = _generatorWalls.GetWalls(floorPositions, Neighbourhood.Full);
            foreach (Vector2Int wall in cornerWalls)
            {
                CornerWallPaint(_wallMap, wall, Neighbourhood.GetHash(wall, floorPositions, Neighbourhood.Full));
            }

        }

        private void BasicWallPaint(Tilemap wallMap, Vector2Int wallPosition, string wallNeighbourhood)
        {
            
            // DEBUG if no wall attributed
            // Debug.Log("Basic : " + wallPosition + " Neighbourhood : " + wallNeighbourhood);

            TileBase tile = null;
            
            int intWallNeighbourhood = Convert.ToInt32(wallNeighbourhood, 2);
            
            if (WallsGenerationHelper.wallTop.Contains(intWallNeighbourhood))
                tile = _upWallTile;
            if (WallsGenerationHelper.wallLeft.Contains(intWallNeighbourhood))
                tile = _leftWallTile;
            if (WallsGenerationHelper.wallBottom.Contains(intWallNeighbourhood))
                tile = _downWallTile;
            if (WallsGenerationHelper.wallRight.Contains(intWallNeighbourhood))
                tile = _rightWallTile;
            
            if(tile != null)
                MapPainter.CellPaint(wallPosition, _wallMap, tile);

        }
        private void CornerWallPaint(Tilemap wallMap, Vector2Int wallPosition, string wallNeighbourhood)
        {
            // DEBUG if no wall attributed
            // Debug.Log("Corner : " + wallPosition + " Neighbourhood : " + wallNeighbourhood);

            TileBase tile = null;
            
            int intWallNeighbourhood = Convert.ToInt32(wallNeighbourhood, 2);
            
            if (WallsGenerationHelper.wallUpRight.Contains(intWallNeighbourhood))
                tile = _upRightWallTile;
            if (WallsGenerationHelper.wallDownRight.Contains(intWallNeighbourhood))
                tile = _downRightWallTile;
            if (WallsGenerationHelper.wallDownLeft.Contains(intWallNeighbourhood))
                tile = _downLeftWallTile;
            if (WallsGenerationHelper.wallUpLeft.Contains(intWallNeighbourhood))
                tile = _upLeftWallTile;
           
            if (WallsGenerationHelper.wallInnerDownRight.Contains(intWallNeighbourhood))
                tile = _innerDownRightWallTile;
            if (WallsGenerationHelper.wallInnerDownLeft.Contains(intWallNeighbourhood))
                tile = _innerDownLeftWallTile;

            // if (WallsGenerationHelper.wallFull.Contains(intWallNeighbourhood))
            //     tile = _fullMapTile;
            
            if(tile != null)
                MapPainter.CellPaint(wallPosition, _wallMap, tile);

        }

        private void Update()
        {
            _treeGraphGenerator.DrawDebug();
        }


    }
}
