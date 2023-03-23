using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.Subsystems;
using Random = UnityEngine.Random;

public class Generator_GameOfLife : MonoBehaviour
{

    // [Header("Space Definition")]
    // [SerializeField] private Vector2Int _size;
    // [SerializeField][Range(0,1)] private float _bordersRatio = 0.0f;
    // [SerializeField] Vector2Int _startPosition;
    //
    // [Header("Parameters")]
    // [SerializeField][Range(0,1)] private float _initialFillRatio = 0.5f;
    // [SerializeField] private int _maxIterations = 200;
    // [SerializeField] private List<int> _stayAliveValues;
    // [SerializeField] private List<int> _comeAliveValues;
    [SerializeField] private SO_GameOfLife_Generator_Settings _settings;
    public SO_GameOfLife_Generator_Settings Settings => _settings;

    private HashSet<Vector2Int> _alivePositions = new HashSet<Vector2Int>();
    private List<Vector2Int> _insidePositions = new List<Vector2Int>();
    private HashSet<Vector2Int> _cavePositions = new HashSet<Vector2Int>();
    private BoundsInt _originalMap = new BoundsInt();
    private BoundsInt _fullMap = new BoundsInt();
    private List<List<Vector2Int>> _floodFills = new List<List<Vector2Int>>();

    public List<Vector2Int> InsidePositions => _insidePositions;

    public HashSet<Vector2Int> GetCave()
    {
        return _cavePositions;
    }

    public List<List<Vector2Int>> GetFloodFills()
    {
        return _floodFills;
    }

    public HashSet<Vector2Int> Generate()
    {
        int oldPositionsCount = 0;
        int nbIterations = 0;
        
        Debug.Log("GAME OF LIFE : Set Maps");
        SetMaps();

        Debug.Log("GAME OF LIFE : Randomize map");
        Initiate();
        
        do
        {
            Debug.Log("GAME OF LIFE : Iteration #" + nbIterations);
            
            oldPositionsCount = _alivePositions.Count;
            _alivePositions = GameOfLifeIteration(_fullMap, _alivePositions);
            nbIterations++;
            
        } while (oldPositionsCount != _alivePositions.Count && nbIterations <=_settings.maxIterations);
        
        Debug.Log("GAME OF LIFE : Invert positions");
        _cavePositions = Invert(_fullMap, _alivePositions);
        
        Debug.Log("GAME OF LIFE : Check flood fill");
        CheckFloodFills();
        if (_floodFills.Count > 0)
        {
            _alivePositions = new HashSet<Vector2Int>(_floodFills.OrderBy(ff => ff.Count).Last());
        }
        else
        {
            _alivePositions = new HashSet<Vector2Int>(_cavePositions);
        }
        
        Debug.Log("GAME OF LIFE : Clean isolated positions");
        CleanIsolatedPositions(_alivePositions, new List<int> {5, 6, 7, 8}, Neighbourhood.Full);
        _insidePositions = _alivePositions.ToList();
        
        ExpandTiles(_alivePositions, Neighbourhood.Cardinals);
        ExpandTiles(_alivePositions, Neighbourhood.Cardinals);
        
        return _alivePositions;

    }

    private void ExpandTiles(HashSet<Vector2Int> map, List<Vector2Int> neighbours)
    {
        List<Vector2Int> expansionTiles = new List<Vector2Int>();
        foreach (Vector2Int p in map)
        {
            foreach (var n in neighbours)    
            {
                if(!map.Contains(n + p))
                    expansionTiles.Add(n +p);
            }
        }
        
        map.UnionWith(expansionTiles);
        
    }

    private void CleanIsolatedPositions(HashSet<Vector2Int> alivePositions, List<int> emptyNeighbours, List<Vector2Int> neighbourhood)
    {

        bool cleanProcessDone = true;
        
        do
        {
            cleanProcessDone = true;
            List<Vector2Int> isolatedPositions = new List<Vector2Int>();
            
            foreach (Vector2Int position in alivePositions)
            {
                int nbCount = neighbourhood.Count - Neighbourhood.GetCount(position, alivePositions, neighbourhood);
                if (emptyNeighbours.Contains(nbCount))
                {
                    cleanProcessDone = false;
                    isolatedPositions.Add(position);
                }
            }

            foreach (Vector2Int position in isolatedPositions)
            {
                alivePositions.Remove(position);
            }
            
        } while (!cleanProcessDone);
        
    }


    
    public List<List<Vector2Int>> CheckFloodFills()
    {

        _floodFills = new List<List<Vector2Int>>();
        
        // Check flood fills
        List<Vector2Int> checkPositions = _cavePositions.ToList();

        do
        {
            List<Vector2Int> oneFloodFill = new List<Vector2Int>();
            Queue<Vector2Int> bfsQueue = new Queue<Vector2Int>();
            
            Vector2Int position = checkPositions.ElementAt(0);
            bfsQueue.Enqueue(position);

            do
            {
                // ------------------------------------------------------------------
                Vector2Int bfsParserPosition = bfsQueue.Dequeue();
                if (!oneFloodFill.Contains(bfsParserPosition))
                {
                    oneFloodFill.Add(bfsParserPosition);
                    checkPositions.Remove(bfsParserPosition);
                }
                
                // Ajout des voisins ----------------------------------------
                foreach (Vector2Int neighbourDirection in Neighbourhood.Cardinals)
                {
                    Vector2Int nextPosition = bfsParserPosition + neighbourDirection;
                    if (checkPositions.Contains(nextPosition))
                    {
                        if (!bfsQueue.Contains(nextPosition))
                        {
                            bfsQueue.Enqueue(nextPosition);
                        }
                    }
                }
                
            } while (bfsQueue.Count > 0);
            
            if (oneFloodFill.Count > 0)
            {
                _floodFills.Add(new List<Vector2Int>(oneFloodFill));
                // foreach (Vector2Int off in oneFloodFill)
                // {
                //     checkPositions.Remove(off);
                // }
            }

            checkPositions.Remove(position);

        } while (checkPositions.Count > 0);

        return _floodFills;

    }

    private void AddFloodFill(Vector2Int position, List<Vector2Int> currentFloodFill)
    {

        foreach (Vector2Int neighbourDirection in Neighbourhood.Cardinals)
        {
            Vector2Int newPosition = position + neighbourDirection;

            if (_cavePositions.Contains(newPosition) && !currentFloodFill.Contains(newPosition))
            {
                currentFloodFill.Add(newPosition);
                AddFloodFill(newPosition, currentFloodFill);
            }
                
        }
        
    }

    private void SetMaps()
    {

        _fullMap = new BoundsInt(
            _settings.startPosition.x - Mathf.FloorToInt(0.5f * _settings.size.x), _settings.startPosition.y - Mathf.FloorToInt(0.5f * _settings.size.y), 0,
            _settings.size.x, _settings.size.y, 0);
        
        _originalMap = new BoundsInt(
            _settings.startPosition.x - Mathf.FloorToInt(0.5f * (1.0f - _settings.bordersRatio) * _settings.size.x), _settings.startPosition.y - Mathf.FloorToInt(0.5f * (1.0f - _settings.bordersRatio) * _settings.size.y), 0,
            Mathf.FloorToInt((1.0f - _settings.bordersRatio) * _settings.size.x),  Mathf.FloorToInt((1.0f - _settings.bordersRatio) * _settings.size.y), 0);

    }

    public HashSet<Vector2Int> GameOfLifeIterate()
    {
        _alivePositions = GameOfLifeIteration(_fullMap, _alivePositions);
        return _alivePositions;
        
    } 
    
    private HashSet<Vector2Int> GameOfLifeIteration(BoundsInt originalMap, HashSet<Vector2Int> alivePositions)
    {
        HashSet<Vector2Int> newPositions = new HashSet<Vector2Int>();
        
        // Iterate through each point, and log it to the Debug Console.
        for(int x = originalMap.xMin; x <= originalMap.xMax; x++)
        {
            for(int y = originalMap.xMin; y <= originalMap.xMax; y++)
            {
                Vector2Int my2DPosition = new Vector2Int(x, y);
            
                // l’état des cases est évalué
                bool isAlive = alivePositions.Contains(my2DPosition);
                // Le nombre de voisin de chaque case est compté
                int neighbourCount = Neighbourhood.GetCount(my2DPosition, alivePositions, Neighbourhood.Full);

                if (isAlive && _settings.stayAliveValues.Contains(neighbourCount))
                {
                    // Si la case est vivante et qu’elle a 2-3 voisins vivant, elle reste vivante
                    newPositions.Add(my2DPosition);
                
                }else if (!isAlive == false && _settings.comeAliveValues.Contains(neighbourCount))
                {
                    // Si la case est morte et qu’elle a 3 voisins vivants, elle devient vivante
                    newPositions.Add(my2DPosition);

                }
                else
                {
                    // Sinon elle considérée comme morte
                    // RIEN
                }
            }
        }

        return newPositions;

    }


    public HashSet<Vector2Int> Initiate()
    {
        
        SetMaps();
        _alivePositions.Clear();
        
        for(int x = _fullMap.xMin; x <= _fullMap.xMax; x++)
        {
            for(int y = _fullMap.xMin; y <= _fullMap.xMax; y++)
            {
                if ((x >= _originalMap.xMin && x <= _originalMap.xMax) &&
                    (y >= _originalMap.yMin && y <= _originalMap.yMax))
                {
                    // if in orginal map, add a position randomly
                    if(Random.value <= _settings.initialFillRatio)
                        _alivePositions.Add(new Vector2Int(x, y));

                }
                else
                {
                    // If not , add it anyway
                    _alivePositions.Add(new Vector2Int(x, y));
                    
                }
            }
        }
        
        return _alivePositions;

    }

    public HashSet<Vector2Int> Invert(BoundsInt referenceMap, HashSet<Vector2Int> positionsToInvert)
    {

        HashSet<Vector2Int> invertedPositions = new HashSet<Vector2Int>();

        for(int x = referenceMap.xMin; x <= referenceMap.xMax; x++)
        {
            for(int y = referenceMap.xMin; y <= referenceMap.xMax; y++)
            {
                Vector2Int my2DPosition = new Vector2Int(x, y);
                
                if (!positionsToInvert.Contains(my2DPosition))
                    invertedPositions.Add(my2DPosition);

            }
        }

        return invertedPositions;

    }


}
