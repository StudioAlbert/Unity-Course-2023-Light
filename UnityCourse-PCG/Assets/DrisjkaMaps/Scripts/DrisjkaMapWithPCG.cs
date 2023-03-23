using System.Collections.Generic;
using System.Linq;
using PCGDungeon;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class DrisjkaMapWithPCG : MonoBehaviour
{

    [Header("PCG")]
    [SerializeField] private Generator_GameOfLife _gameOfLifeGenerator;
    [SerializeField] private Tilemap _pcgTileMap;
    [SerializeField] private RuleTile _ruledTile;

    [Header("Disjktra")]
    [SerializeField] private DrisjkaMapGenerator _dijsktraGenerator;
    [SerializeField] private Tilemap _disjktraTileMap;
    [SerializeField] private TileBase _goalsTile;

    [Header("Gradient")]
    [SerializeField] private List<TileBase> _gradientTiles;

    [Header("A Star")] 
    [SerializeField] private AStarPathFinder _aStarPathFinder;
    [SerializeField] private Vector2Int _startAStar;
    [SerializeField] private Vector2Int _endAStar;
    [SerializeField] private Tilemap _aStarPathMap;
    [SerializeField] private TileBase _openTile;
    [SerializeField] private TileBase _closedTile;
    [SerializeField] private TileBase _pathTile;
    [SerializeField] private TileBase _currentNodeTile;

    List<Vector2Int> _PCGMap = new List<Vector2Int>();
    List<Vector2Int> _InsidePositionsMap = new List<Vector2Int>();
    private HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
    
    private void Start()
    { 
        
    }

    public void ClearMaps()
    {
        _disjktraTileMap.ClearAllTiles();
        _pcgTileMap.ClearAllTiles();
        _aStarPathMap.ClearAllTiles();
        _aStarPathFinder.NbIterationsMax = 0;
    }

    public void DoIt()
    {
        ClearMaps();
        GeneratePCGMap();
        GenerateDisjktraMap();
    }
    
    public void GeneratePCGMap()
    {
        ClearMaps();

        floorPositions = _gameOfLifeGenerator.Generate();
        _PCGMap = floorPositions.ToList();
        MapPainter.MapPaint(_PCGMap, _pcgTileMap, _ruledTile);

        _InsidePositionsMap = _gameOfLifeGenerator.InsidePositions;
        
        _startAStar = _InsidePositionsMap.OrderBy(m => Random.value).ElementAt(0);
        _endAStar = _InsidePositionsMap.Where(m => Vector2Int.Distance(_startAStar,m) > 20).OrderBy(m => Random.value).FirstOrDefault();
        
        MapPainter.CellPaint(_startAStar, _aStarPathMap, _goalsTile);
        MapPainter.CellPaint(_endAStar, _aStarPathMap, _goalsTile);

    }
    public void GenerateDisjktraMap()
    {
        SetupDrisjkaGenerator();
        DoTheDrisjkaMap();
    }
    private void SetupDrisjkaGenerator()
    {
        
        _dijsktraGenerator.SetTheMap(_PCGMap);

        // Get random goals ------------------------------------------------------
        _dijsktraGenerator.EmptyGoals();
        List<Vector2Int> somePositions = _PCGMap.OrderBy(p => Random.value).Take(3).ToList();

        foreach (Vector2Int position in somePositions)
        {
            _dijsktraGenerator.AddGoal(position, 0);
        }
        
    }
    private void DoTheDrisjkaMap()
    {
        _dijsktraGenerator.GenerateMultiGoal();
        PaintDrisjkaMap();
    }

    // TO DO : Refactor to become a generic map painter
    private void PaintDrisjkaMap()
    {

        if (_dijsktraGenerator.DrisjkaMap.Count > 0)
        {
            _disjktraTileMap.ClearAllTiles();

            foreach (KeyValuePair<Vector2Int, int> drisjkaPoint in _dijsktraGenerator.DrisjkaMap)
            {
                MapPainter.CellPaint(drisjkaPoint.Key, _disjktraTileMap,
                    MapPainter.PickGradientColorClamp(_gradientTiles, drisjkaPoint.Value));
            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(
            new Vector3(_gameOfLifeGenerator.Settings.startPosition.x, _gameOfLifeGenerator.Settings.startPosition.y, 0),
            new Vector3(_gameOfLifeGenerator.Settings.size.x, _gameOfLifeGenerator.Settings.size.y, 0));
        
        // MapPainter.CellPaint(_startAStar, _aStarPathMap, _goalsTile);
        // MapPainter.CellPaint(_endAStar, _aStarPathMap, _goalsTile);
        
    }

    private void Update()
    {
        //DoTheDrisjkaMap();
    }

    public void DoAStar()
    {
        _aStarPathFinder.GetPath(_InsidePositionsMap, _startAStar, _endAStar);

        _aStarPathMap.ClearAllTiles();
        foreach (var node in _aStarPathFinder.Open)
        {
            MapPainter.CellPaint(node.Position, _aStarPathMap, _openTile);
        }
        foreach (var node in _aStarPathFinder.Closed)
        {
            MapPainter.CellPaint(node.Position, _aStarPathMap, _closedTile);
        }
        foreach (var node in _aStarPathFinder.Path)
        {
            MapPainter.CellPaint(node.Position, _aStarPathMap, _pathTile);
        }
        
        MapPainter.CellPaint(_startAStar, _aStarPathMap, _goalsTile);
        MapPainter.CellPaint(_endAStar, _aStarPathMap, _goalsTile);
        MapPainter.CellPaint(_aStarPathFinder.CurrentNode.Position, _aStarPathMap, _currentNodeTile);

     }

    public void DoAStarIteration()
    {
        _aStarPathFinder.NbIterationsMax++;
        DoAStar();
    }
}


