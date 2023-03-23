using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PCGDungeon
{
    public class PCGDungeon_GeneratorRandomWalk : MonoBehaviour
    {

        [SerializeField] private Vector2Int _startPosition;
        [SerializeField] private int _walkLength = 10;
        //[SerializeField] private int _iterations = 10;
        [SerializeField] private int _minAreaGuaranteed = 40;
        [SerializeField] private bool _restartAtRandomPositions = false;
        [SerializeField] private int _corridorsCount = 6;
        [SerializeField] private int _oneCorridorLength = 25;
        [SerializeField][Range(0,1)] private float _corridorRandomness = 0.25f;
        [SerializeField][Range(0,1)] private float _roomRatio = 0.5f;

        public HashSet<Vector2Int> GenerateSimpleRoom()
        {
            return SimpleRoom(_startPosition);
        }
        public HashSet<Vector2Int> GenerateWithCorridors()
        {
            HashSet<Vector2Int> floors = new HashSet<Vector2Int>();
            List<Vector2Int> roomsPositions = new List<Vector2Int>();
            
            CreateCorridors(_startPosition, floors, roomsPositions);
            
            // Create wayends first
            List<Vector2Int> wayends = new List<Vector2Int>();
            wayends = roomsPositions.FindAll(r => Neighbourhood.GetCount(r, floors, Neighbourhood.Cardinals) <= 1);

            foreach (var roomsPosition in roomsPositions)
                Debug.Log("Room : " + roomsPosition + "Count nieghbourhood : " + Neighbourhood.GetCount(roomsPosition, floors, Neighbourhood.Cardinals));

            foreach (var wayend in wayends)
                Debug.Log("Wayend : " + wayend);
            
            foreach (var position in wayends)
            {
                floors.UnionWith(SimpleRoom(position));
                roomsPositions.Remove(position);
            }
            
            // Create more rooms, number depending on parameter
            int nbRooms = Mathf.FloorToInt(_roomRatio * roomsPositions.Count);
            for (int i = 0; i < nbRooms; i++)
            {
                floors.UnionWith(SimpleRoom(roomsPositions.ElementAt(i)));
            }
           
            return floors;

        }

        private void CreateCorridors(Vector2Int startPosition, HashSet<Vector2Int> floors, List<Vector2Int> roomsPositions)
        {
            Vector2Int position = startPosition;
            roomsPositions.Add(startPosition);
            
            for (int idxCorridor = 0; idxCorridor < _corridorsCount; idxCorridor++)
            {
                Vector2Int direction = Neighbourhood.GetRandomVonNeumannNeighbour();
                HashSet<Vector2Int> oneCorridor = new HashSet<Vector2Int>();

                float newLength = Random.Range(_oneCorridorLength - (_corridorRandomness * _oneCorridorLength),
                    _oneCorridorLength + (_corridorRandomness * _oneCorridorLength));
                
                for (int idxPosition = 0; idxPosition < newLength; idxPosition++)
                {
                    position += direction;
                    floors.Add(position);
                }
                //floors.UnionWith(oneCorridor);
                roomsPositions.Add(position);
                
            }
            
        }

        private HashSet<Vector2Int> SimpleRoom(Vector2Int startPosition)
        {

            Vector2Int currentPosition = startPosition;
            HashSet<Vector2Int> positions = new HashSet<Vector2Int>();

            //for (int iter = 0; iter < _iterations; iter++)
            while (positions.Count <= _minAreaGuaranteed)
            {
                positions.UnionWith(SimpleRandomWalk(currentPosition, _walkLength));
                if (_restartAtRandomPositions)
                    currentPosition = positions.ElementAt(Random.Range(0, positions.Count));

            }

            return positions;
        }

        private HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPosition, int walkLength)
        {
            
            HashSet<Vector2Int> theWalk = new HashSet<Vector2Int>();
            
            theWalk.Add(startPosition);
            Vector2Int previousPosition = startPosition;

            for (int step = 0; step < walkLength; step++)
            {
                Vector2Int newPosition = previousPosition + Neighbourhood.GetRandomVonNeumannNeighbour();
                theWalk.Add(newPosition);
                previousPosition = newPosition;
            }
            
            return theWalk;
        }

        public HashSet<Vector2Int> PatchSurroundedTiles(HashSet<Vector2Int> wallsPositions, HashSet<Vector2Int> floorPositions, List<Vector2Int> neighbourhood, float neighbourRatio)
        {

            HashSet<Vector2Int> patchPositions = new HashSet<Vector2Int>();

            foreach (Vector2Int patchCandidate in wallsPositions)
            {
                int countNeighbours = 0;
                
                // Count the neighbours
                foreach (Vector2Int neighbour in neighbourhood)
                {
                    Vector2Int pos = patchCandidate + neighbour;
                    if (floorPositions.Contains(pos))
                        countNeighbours++;
                }
                
                // If too mor neighbours add to floor positions
                if (countNeighbours >= (neighbourRatio * neighbourhood.Count))
                    patchPositions.Add(patchCandidate);
                
            }

            floorPositions.UnionWith(patchPositions);
            
            return patchPositions;

        }
    }
}
