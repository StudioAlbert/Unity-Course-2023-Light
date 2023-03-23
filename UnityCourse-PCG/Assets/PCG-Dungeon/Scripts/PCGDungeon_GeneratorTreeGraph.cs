using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

namespace PCGDungeon
{
    public class PCGDungeon_GeneratorTreeGraph : MonoBehaviour
    {

        [SerializeField] private Vector2Int _startPosition;
        [SerializeField] private int xSize = 10;
        [SerializeField] private int ySize = 10;
        [SerializeField] 
        [Range(0,1)]
        private float xShrink = 0.95f;
        [SerializeField] 
        [Range(0,1)]
        private float yShrink = 0.95f;
        [SerializeField] 
        [Range(0,1)]
        private float zShrink = 0.95f;

        private TreeGraph _treeGraph = new TreeGraph();
        
        private List<BoundsInt> _rooms = new List<BoundsInt>();
        private BoundsInt _originalRoom = new BoundsInt();

        
        // Update is called once per frame
        public void DrawDebug()
        {
            BoundsSplitter.DrawDebug(_rooms, new Vector3(xShrink,yShrink,zShrink));            
        }
        
        public HashSet<Vector2Int> Generate()
        {
            Vector2Int currentPosition = _startPosition;
            HashSet<Vector2Int> positions = new HashSet<Vector2Int>();

            _rooms = new List<BoundsInt>();
            _originalRoom = new BoundsInt(
                new Vector3Int(
                    _startPosition.x - Mathf.FloorToInt(0.5f * xSize), 
                    _startPosition.y - Mathf.FloorToInt(0.5f * ySize), 
                    0), 
                new Vector3Int(
                    xSize, 
                    ySize, 
                    0));

            _treeGraph.Initiate("tree", 0.5f, BoundsSplitter.SplitDirection.Horizontal);
            _treeGraph.Process(_originalRoom, _rooms);

            foreach (BoundsInt room in _rooms)
            {
                BoundsInt shrinkedRoom = BoundsSplitter.ShrinkedRoom(room, new Vector3(xShrink,yShrink,zShrink));
                positions.UnionWith(BoundsToPositions(shrinkedRoom));
            }

            return positions;
        }
        
        private IEnumerable<Vector2Int> BoundsToPositions(BoundsInt room)
        {
            
            HashSet<Vector2Int> positions = new HashSet<Vector2Int>();

            for (int myX = room.xMin; myX < room.xMax; myX++)
            {
                for (int myY = room.yMin; myY < room.yMax; myY++)
                {
                    Vector2Int newPosition = new Vector2Int(myX, myY);
                    positions.Add(newPosition);
                }
            }

            return positions;

        }
        
    }
}
