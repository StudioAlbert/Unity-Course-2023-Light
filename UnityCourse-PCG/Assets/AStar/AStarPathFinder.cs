using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AStarPathFinder : MonoBehaviour
{

    [SerializeField] private int _nbIterationsMax;
    [SerializeField] 
    [Range(0.1f, 2)]private float _heuristicFactor = 1f;

    private List<AStarNode> _open = new List<AStarNode>();
    private List<AStarNode> _closed = new List<AStarNode>();
    private List<AStarNode> _path = new List<AStarNode>();
    private Vector2Int _start;
    private Vector2Int _end;
    private AStarNode _currentNode;

    public List<AStarNode> Open => _open;
    public List<AStarNode> Closed => _closed;
    public List<AStarNode> Path => _path;
    public AStarNode CurrentNode => _currentNode;
    public int NbIterationsMax { get => _nbIterationsMax; set => _nbIterationsMax = value; }

    public void GetPath(List<Vector2Int> map, Vector2Int start, Vector2Int end)
    {
        _open = new List<AStarNode>();
        _closed = new List<AStarNode>();
        _path = new List<AStarNode>();

        _start = start;
        _end = end;
        if (!map.Contains(_start))
        {
            Debug.LogWarning(_start + " not found");
        }
        if (!map.Contains(_end))
        {
            Debug.LogWarning(_end + " not found");
        }

        _currentNode = new AStarNode(_start, Vector2Int.Distance(_start, _end), 0, null);

        int nbIterations = 0;
        
        while (_currentNode.Position != _end && nbIterations < _nbIterationsMax)
        {
            // ----------------------------------------------------------
            foreach (var n in Neighbourhood.Full)
            {
                Vector2Int newPos = _currentNode.Position + n;
                
                AStarNode newNode = new AStarNode(
                    newPos,
                    Vector2Int.Distance(newPos, _end) * _heuristicFactor,
                    _currentNode.G + 1,
                    _currentNode);

                if (map.Contains(newPos) && !_open.Exists(n => n.Position == newPos) && !_closed.Exists(n => n.Position == newPos))
                {
                    _open.Add(newNode);
                }
            }

            _currentNode = _open.OrderBy(o => o.F).ElementAt(0); 
            _closed.Add(_currentNode);
            _open.Remove(_currentNode);

            nbIterations++;

        }
        
        
        
        // Get the path
        AStarNode rollbacknode = new AStarNode(_currentNode);
        while (rollbacknode.Position != _start)
        {
            rollbacknode = rollbacknode.Parent;
            _path.Add(rollbacknode);
        }

    }

}

public class AStarNode
{

    private Vector2Int _position;
    private float _H;
    private float _G;
    private AStarNode _parent;

    public float H { get => _H; set => _H = value; }
    public float G { get => _G; set => _G = value; }
    public AStarNode Parent { get => _parent; set => _parent = value; }
    public Vector2Int Position { get => _position; set => _position = value; }

    public AStarNode(Vector2Int p, float h, float g, AStarNode parent)
    {
        _position = p;
        _H = h;
        _G = g;
        _parent = parent;
    }    
    public AStarNode(AStarNode copy)
    {
        _position = copy._position;
        _H = copy._H;
        _G = copy._G;
        _parent = copy._parent;
    }
    
    public float F
    {
        get
        {
            return _H + _G;
        }
    }

}
