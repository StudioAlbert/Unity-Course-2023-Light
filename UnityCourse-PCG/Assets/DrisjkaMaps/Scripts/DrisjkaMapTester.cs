using System;
using System.Collections;
using System.Collections.Generic;
using PCGDungeon;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class DrisjkaMapTester : MonoBehaviour
{

    
    [SerializeField] private DrisjkaMapGenerator _generator;
    [SerializeField] private Vector2Int _size;
    [SerializeField] private Vector2Int _startPosition;
    [SerializeField] private List<Vector2Int> _goals;
    [SerializeField] private List<int> _goalsPriorities;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase _pathTile;

    [Header("Gradient")]
    [SerializeField] private List<TileBase> _gradientTiles;

    private void Start()
    { 
        SetupDrisjkaGenerator();
        DoTheDrisjkaMapMultiGoal();
    }

    public void ClearMaps()
    {
        _tilemap.ClearAllTiles();
    }
    public void SetAndDoDrisjakMapOneGoal()
    { 
        SetupDrisjkaGenerator();
        DoTheDrisjkaMapOneGoal();
    }
    public void SetAndDoDrisjakMapMultiGoals()
    { 
        SetupDrisjkaGenerator();
        DoTheDrisjkaMapMultiGoal();
    }
    private void SetupDrisjkaGenerator()
    {
        List<Vector2Int> _fullMap = new List<Vector2Int>();

        _fullMap.Clear();
        // Get a rectangle map based on size
        for (int x = (_startPosition.x - Mathf.FloorToInt(_size.x / 2));
             x < (_startPosition.x + Mathf.FloorToInt(_size.x / 2));
             x++)
        {
            for (int y = (_startPosition.x - Mathf.FloorToInt(_size.y / 2));
                 y < (_startPosition.y + Mathf.FloorToInt(_size.y / 2));
                 y++)
            {
                _fullMap.Add(new Vector2Int(x, y));
            }
        }
        _generator.SetTheMap(_fullMap);

        _generator.EmptyGoals();
        for (int goalIdx = 0; goalIdx < _goals.Count; goalIdx++)
        {
            int priority = 0;
            if (goalIdx < _goalsPriorities.Count)
                priority = _goalsPriorities[goalIdx];

            _generator.AddGoal(_goals[goalIdx], priority);

        }
    }
    private void DoTheDrisjkaMapOneGoal()
    {
        if (_goals.Count != 0)
        {
            _generator.GenerateOneGoal();
            PaintDrisjkaMap();
        }

    }
    private void DoTheDrisjkaMapMultiGoal()
    {
        if(_goals.Count > 1)
        {
            _generator.GenerateMultiGoal();
            PaintDrisjkaMap();
        }
    }

    private void PaintDrisjkaMap()
    {

        if (_generator.DrisjkaMap.Count > 0)
        {
            _tilemap.ClearAllTiles();

            foreach (KeyValuePair<Vector2Int, int> drisjkaPoint in _generator.DrisjkaMap)
            {
                MapPainter.CellPaint(drisjkaPoint.Key, _tilemap,
                    MapPainter.PickGradientColorClamp(_gradientTiles, drisjkaPoint.Value));
            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(new Vector3(_startPosition.x, _startPosition.y, 0), new Vector3(_size.x, _size.y, 0));
    }

    private void Update()
    {
        //DoTheDrisjkaMap();
    }
}


