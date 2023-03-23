using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PCGDungeon
{
    public class PCGDungeon_GeneratorBSPAreaGuaranteed : MonoBehaviour
    {
        [SerializeField] private Vector2Int _startPosition;
        [SerializeField] private Vector2Int _size = new Vector2Int(10,10);
        [SerializeField] [Range(0,1)] private float _shrink = 0.95f;
        [SerializeField] private float _minWidth = 25;
        [SerializeField] private float _minHeight = 25;
        [SerializeField][Range(0,1)] private float _minCutRatio = 0.1f;
        [SerializeField][Range(0,1)] private float _maxCutRatio = 0.9f;
        [SerializeField] private float _XYDifference = 10f;
        [SerializeField] private BSPAreaGuaranteed.DirectionPickerMode _directionPickerMode = BSPAreaGuaranteed.DirectionPickerMode.AmongBounds;
        
        // [SerializeField] 
        // [Range(0,1)]
        // private float xShrink = 0.95f;
        // [SerializeField] 
        // [Range(0,1)]
        // private float yShrink = 0.95f;
        // [SerializeField] 
        // [Range(0,1)]
        // private float zShrink = 0.95f;
        
        private BSPAreaGuaranteed _bsp = new BSPAreaGuaranteed();
        
        public HashSet<Vector2Int> Generate()
        {
            HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
            HashSet<Vector2Int> corridorsPositions = new HashSet<Vector2Int>();
            
            List<BoundsInt> _rooms = new List<BoundsInt>();
            BoundsInt _originalRoom = new BoundsInt(
                new Vector3Int(
                    _startPosition.x - Mathf.FloorToInt(0.5f * _size.x), 
                    _startPosition.y - Mathf.FloorToInt(0.5f * _size.y), 
                    0), 
                new Vector3Int(
                    _size.x, 
                    _size.y, 
                    0));
            List<Vector2Int> roomCenters = new List<Vector2Int>();


            _bsp.Process(_originalRoom, _rooms, _directionPickerMode, _minWidth, _minHeight, _minCutRatio, _maxCutRatio, _XYDifference);
            foreach (BoundsInt room in _rooms)
            {
                // Add positions
                floorPositions.UnionWith(BoundsToPositions(room));
                // Collect centers
                roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
            }

            corridorsPositions = ConnectRooms(roomCenters);
            floorPositions.UnionWith(corridorsPositions);            

            return floorPositions;

        }

        private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
        {
            HashSet<Vector2Int> roomConnections = new HashSet<Vector2Int>();
            Vector2Int currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
            Vector2Int closestCenter;
            
            while (roomCenters.Count > 0)
            {
                List<Vector2Int> centersSort = roomCenters.OrderBy(c => Vector2Int.Distance(c, currentRoomCenter)).ToList();
                closestCenter = centersSort[0]; 
                
                // float minDist = Mathf.Infinity;
                //
                // foreach (var rc in roomCenters)
                // {
                //     float myDistance = Vector2.Distance(rc, currentRoomCenter);
                //     if (myDistance < minDist)
                //     {
                //         closestCenter = rc;
                //         minDist = myDistance;
                //     }
                // }
                
                HashSet<Vector2Int> oneRoomConnections = OneRoomConnection(currentRoomCenter, closestCenter);
                
                // Add corridor to all corridors
                roomConnections.UnionWith(oneRoomConnections);
                // Remove the center processed to the list
                roomCenters.Remove(currentRoomCenter);
                // the next center to process is the closest one
                currentRoomCenter = closestCenter;
                
            }

            return roomConnections;

        }

        private HashSet<Vector2Int> OneRoomConnection(Vector2Int origin, Vector2Int destination)
        {
            HashSet<Vector2Int> roomConnection = new HashSet<Vector2Int>();

            var position = origin;
            // Move left or right and find X match between origin and destination
            do
            {
                roomConnection.Add(position);
                if(position.x < destination.x)
                    position += Vector2Int.right;
                else if (position.x > destination.x)
                    position += Vector2Int.left;
                    
            } while (position.x != destination.x);
            
            // Move up or down and find Y match between origin and destination
            do
            {
                roomConnection.Add(position);
                if(position.y < destination.y)
                    position += Vector2Int.up;
                else if (position.y > destination.y)
                    position += Vector2Int.down;
                    
            } while (position.y != destination.y);

            return roomConnection;
            
        }

        private IEnumerable<Vector2Int> BoundsToPositions(BoundsInt room)
        {
            
            BoundsInt shrinkedRoom = BoundsSplitter.ShrinkedRoom(room, new Vector3(_shrink,_shrink,_shrink));

            HashSet<Vector2Int> positions = new HashSet<Vector2Int>();

            for (int myX = shrinkedRoom.xMin; myX < shrinkedRoom.xMax; myX++)
            {
                for (int myY = shrinkedRoom.yMin; myY < shrinkedRoom.yMax; myY++)
                {
                    Vector2Int newPosition = new Vector2Int(myX, myY);
                    positions.Add(newPosition);
                }
            }

            return positions;

        }

    }
}
