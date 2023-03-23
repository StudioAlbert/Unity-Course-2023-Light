using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding : MonoBehaviour
{

    [SerializeField] private Tilemap _floor;
    [SerializeField] private Tilemap _debug;

    [SerializeField] private TileBase _debugTile;
    [SerializeField] private TileBase _startTile;
    [SerializeField] private TileBase _endTile;
    [SerializeField] private TileBase _pathTile;

    [SerializeField] private Vector2Int _startPosition = Vector2Int.zero;
    [SerializeField] private Vector2Int _endPosition = Vector2Int.zero;
    
    private HashSet<Vector2Int> _availablePositions = new HashSet<Vector2Int>();

    private List<Vector2Int> _neighbours = new List<Vector2Int>
    {
        new Vector2Int(0,1),
        new Vector2Int(1,0),
        new Vector2Int(0,-1),
        new Vector2Int(-1,0)
    };
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("DijkstraSearch");
    }

    private IEnumerator BFSSearch()
    {

        bool foundExit = false;

        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        stack.Push(_startPosition);

        _debug.SetTile(new Vector3Int(_startPosition.x, _startPosition.y, 0), _startTile);
        _debug.SetTile(new Vector3Int(_endPosition.x, _endPosition.y, 0), _endTile);

        do
        {
            Vector2Int _refPosition = stack.Pop();

            foreach (var neighbour in _neighbours)
            {

                if (_refPosition == _endPosition)
                {
                    foundExit = true;
                }
                else
                {
                    Vector2Int pos = _refPosition + neighbour;
                    if (!_availablePositions.Contains(pos) && IsPositionAvailable(_floor, pos))
                    {
                        _availablePositions.Add(pos);
                        stack.Push(pos);

                        // Debug -------------------
                        if (pos != _startPosition && pos != _endPosition)
                            _debug.SetTile(new Vector3Int(pos.x, pos.y, 0), _debugTile);

                        yield return new WaitForSeconds(0.1f);

                    }

                }

            }

        } while (stack.Count() > 0 && !foundExit);

        Debug.Log("End of path research....");
        if (foundExit)
            Debug.Log("Found the exit !!!");
    }

    struct DijkstraPoint
    {
        public DijkstraPoint(Vector2Int pos, float distance)
        {
            Pos = pos;
            Distance = distance;
        }
        
        public Vector2Int Pos;
        public float Distance;
    }
    
    private IEnumerator DijkstraSearch()
    {

        bool foundExit = false;

        Queue<DijkstraPoint> queue = new Queue<DijkstraPoint>();
        Dictionary<Vector2Int, float> _availableDistances = new Dictionary<Vector2Int, float>();

        DijkstraPoint foundPoint = new DijkstraPoint();
        
        queue.Enqueue(new DijkstraPoint(_startPosition, 0));
        _availableDistances[_startPosition] = 0;
        
        _debug.SetTile(new Vector3Int(_startPosition.x, _startPosition.y, 0), _startTile);
        _debug.SetTile(new Vector3Int(_endPosition.x, _endPosition.y, 0), _endTile);

        do
        {
            DijkstraPoint _refPoint = queue.Dequeue();

            foreach (var neighbour in _neighbours)
            {
                Vector2Int pos = _refPoint.Pos + neighbour;
                float distance = _refPoint.Distance + Vector2.Distance(pos, _refPoint.Pos);

                if (pos == _endPosition)
                {
                    foundExit = true;
                    foundPoint = new DijkstraPoint(pos, distance);
                    break;
                }
                else
                {

                    if (IsPositionAvailable(_floor, pos))
                    {

                        if (_availableDistances.ContainsKey(pos))
                        {
                            if(distance < _availableDistances[pos])
                                _availableDistances[pos] = distance;
                        }
                        else
                        {
                            _availableDistances[pos] = distance;
                            queue.Enqueue(new DijkstraPoint(pos, distance));
                       }

                        // Debug -------------------
                        if (pos != _startPosition && pos != _endPosition)
                            _debug.SetTile(new Vector3Int(pos.x, pos.y, 0), _debugTile);

                        yield return new WaitForSeconds(0.1f);

                    }

                }

            }

        } while (queue.Count() > 0 && !foundExit);

        Debug.Log("End of path research....");
        
        if (foundExit)
        {
            Debug.Log("Found the exit !!!");

            List<DijkstraPoint> path = new List<DijkstraPoint>();
            Vector2Int queryPoint = foundPoint.Pos;

            float queryDistance = float.MaxValue;
            Vector2Int queryPos = new Vector2Int();

           do
            {
                
                foreach (var neighbour in _neighbours)
                {

                    Vector2Int pos = queryPoint + neighbour;

                    if (_availableDistances.ContainsKey(pos))
                    {
                        if (_availableDistances[pos] < queryDistance)
                        {
                            queryDistance = _availableDistances[pos];
                            queryPos = pos;
                        }
                    }

                }
            
                path.Add(new DijkstraPoint(queryPos, queryDistance));
                _debug.SetTile(new Vector3Int(queryPos.x, queryPos.y, 0), _pathTile);
                queryPoint = queryPos;

                yield return new WaitForSeconds(0.1f);

            } while (queryPoint != _startPosition);
            
        }
        else
        {
            Debug.Log("No way out !!!!!");
        }
    }

    // Update is called once per frame
    void Update()
    {


    }

    private bool IsPositionAvailable(Tilemap _map, Vector2Int _position)
    {
        return _map.HasTile(new Vector3Int(_position.x, _position.y, 0));
    }
}
