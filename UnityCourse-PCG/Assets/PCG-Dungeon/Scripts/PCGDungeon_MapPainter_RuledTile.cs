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
    public class PCGDungeon_MapPainter_RuledTile : MonoBehaviour
    {
        [Header("Maps")]
        [SerializeField] private Tilemap _floorMap;
        [SerializeField] private Tilemap _wallMap;
        [SerializeField] private Tilemap _patchMap;
        
        [Header("Tiles")]
        [SerializeField] private RuleTile _ruledTile;
        [SerializeField] private TileBase _defaultTile;
        
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
            floorPositions.UnionWith(patchesA);
            
            HashSet<Vector2Int> patchesB = _rndWalkGenerator.PatchSurroundedTiles(basicWalls, floorPositions, Neighbourhood.Cardinals, 0.5f);
            floorPositions.UnionWith(patchesB);
 
            MapPaint(floorPositions, _floorMap, _ruledTile);
            
        }
        
        public void CorridorsFirstGenerate()
        {
            // Create the floor --
            _floorMap.ClearAllTiles();
            
            floorPositions = _rndWalkGenerator.GenerateWithCorridors();
            
            HashSet<Vector2Int> basicWalls = _generatorWalls.GetWalls(floorPositions, Neighbourhood.Cardinals);
            floorPositions.UnionWith(basicWalls);
            
            MapPaint(floorPositions, _floorMap, _ruledTile);
            
        }
        
        public void TreeGraphGenerate()
        {
            // HashSet<Vector2Int> floorPositions;

            // Create the floor --
            _floorMap.ClearAllTiles();
            floorPositions = _treeGraphGenerator.Generate();
            MapPaint(floorPositions, _floorMap, _ruledTile);
            
        }
        
        public void BSPAreaGenerate()
        {
            // HashSet<Vector2Int> floorPositions;

            // Create the floor --
            _floorMap.ClearAllTiles();
            floorPositions = _bspAreaGenerator.Generate();
            
            HashSet<Vector2Int> basicWalls = _generatorWalls.GetWalls(floorPositions, Neighbourhood.Full);
            floorPositions.UnionWith(basicWalls);
            
            MapPaint(floorPositions, _floorMap, _ruledTile);
            
        }

        public void GameOfLifeGenerate()
        {
            _floorMap.ClearAllTiles();
            _wallMap.ClearAllTiles();
            _patchMap.ClearAllTiles();
            
            floorPositions = _gameOfLifeGenerator.Generate();
            MapPaint(floorPositions, _floorMap, _ruledTile);
            
            HashSet<Vector2Int> cavePositions = _gameOfLifeGenerator.GetCave();
            MapPaint(cavePositions, _wallMap, _defaultTile);
            
            var floodFills = _gameOfLifeGenerator.GetFloodFills();
            int gradientIndex = 0;
            foreach (var floodFill in floodFills)
            {
                MapPaint(floodFill, _patchMap, PickGradientColor(gradientIndex++));
            }
            
        }

        private TileBase PickGradientColor(int index)
        {
            int gradientIdx = index % (_gradient.Count - 1);
            return _gradient[gradientIdx];
        }

        public void GameOfLifeIterate()
        {
            _wallMap.ClearAllTiles();
            floorPositions = _gameOfLifeGenerator.GameOfLifeIterate();
            MapPaint(floorPositions, _wallMap, _ruledTile);

        }
        
        public void GameOfLifeReset()
        {
            _wallMap.ClearAllTiles();
            floorPositions = _gameOfLifeGenerator.Initiate();
            MapPaint(floorPositions, _wallMap, _ruledTile);
            
        }

        private void MapPaint(IEnumerable<Vector2Int> positions, Tilemap map, TileBase tile)
        {
            foreach (Vector2Int position in positions)
            {
                CellPaint(position, map, tile);
            }

        }
        private void MapPaint(IEnumerable<Vector2Int> positions, Tilemap map, RuleTile tile)
        {
            foreach (Vector2Int position in positions)
            {
                CellPaint(position, map, tile);
            }
        }

        private static void CellPaint(Vector2Int position, Tilemap map, TileBase tile)
        {
            var mapPosition = map.WorldToCell((Vector3Int)position);
            map.SetTile(mapPosition, tile);
        }
        private static void CellPaint(Vector2Int position, Tilemap map, RuleTile tile)
        {
            var mapPosition = map.WorldToCell((Vector3Int)position);
            map.SetTile(mapPosition, tile);
        }
        private void Update()
        {
            //_treeGraphGenerator.DrawDebug();
        }


    }
}
