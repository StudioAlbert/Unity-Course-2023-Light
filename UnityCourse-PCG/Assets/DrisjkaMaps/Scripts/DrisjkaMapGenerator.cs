using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

//using Random = System.Random;

public class DrisjkaMapGenerator : MonoBehaviour
{

   private Dictionary<Vector2Int, int> _drisjkaMap = new Dictionary<Vector2Int, int>();
   private List<DrisjkaGoal> _goals = new List<DrisjkaGoal>();
   private int _lessPriorityValue = Int32.MaxValue;
   private IEnumerator _doTheDrisjkaMap;
   private List<Vector2Int> _mapPositions = new List<Vector2Int>();
   
   public Dictionary<Vector2Int, int> DrisjkaMap => _drisjkaMap;

   [SerializeField] private DrisjkaMapSettings _settings;

   private void Start()
   {
   }

   public void EmptyGoals()
   {
      _goals.Clear();
   }
   public void AddGoal(Vector2Int position, int priority)
   {
      if(priority <= 0)
      {
         _goals.Add(new DrisjkaGoal(position, priority));
         if (priority < _lessPriorityValue)
            _lessPriorityValue = priority;
      }      
      else
      {
         Debug.LogWarning("Priority of goal " + position + " is more than zero. Goal not added");
      }   
      
   }
   public void SetTheMap(List<Vector2Int> mapPositions)
   {
      _mapPositions = new List<Vector2Int>(mapPositions);
   }


   public void GenerateOneGoal()
   {
      _drisjkaMap = DoTheDrisjkaMapOneGoal(_goals[0]);
      
      // StopAllCoroutines();
      // StartCoroutine(_doTheDrisjkaMap);
   }
   public void GenerateMultiGoal()
   {
      _drisjkaMap = DoTheDrisjkaMapMultiGoals();

      // StopAllCoroutines();
      // StartCoroutine(_doTheDrisjkaMap);
   }

   private Dictionary<Vector2Int, int> DoTheDrisjkaMapOneGoal(DrisjkaGoal goal)
   {
      
      Dictionary<Vector2Int, int> tempDrisjkaMap = new Dictionary<Vector2Int, int>();
      
      // List of every positions, the list is emptied along the BFS journey
      List<Vector2Int> checkedPositions = new List<Vector2Int>(_mapPositions);

      // BFS queue : every position inspected go in the queue
      Queue<KeyValuePair<Vector2Int, int>> _tempQueuePositions = new Queue<KeyValuePair<Vector2Int, int>>();

      // Check if goal is inside the map
      if (!_mapPositions.Contains(goal.Position))
      {
         Debug.LogWarning("goal " + goal.ToString() + " does not belong to the global map");
         return tempDrisjkaMap;
      }      
      
      // Add the zero in the queue
      _tempQueuePositions.Enqueue(new KeyValuePair<Vector2Int, int>(goal.Position, goal.Priority));
      checkedPositions.Remove(goal.Position);
      
      // Et zé parti
      while (_tempQueuePositions.Count > 0)
      {
         // Dequeue the last point
         KeyValuePair<Vector2Int, int> point = _tempQueuePositions.Dequeue();
         

         // Here is process  --  --  --  --  --  --  --  --  --  --  --  --  --  --  --  --  --  --
         SetValue(tempDrisjkaMap, _mapPositions, Neighbourhood.Full, point.Key, point.Value);

         // --  --  --  --  --  --  --  --  --  --  --  --  --  --  --  --  --  --  --  --  --  -- 
         foreach (var neighbour in Neighbourhood.Full)
         {
            Vector2Int newPosition = neighbour + point.Key;
            if (checkedPositions.Contains(newPosition))
            {
               // Still in the map
               _tempQueuePositions.Enqueue(new KeyValuePair<Vector2Int, int>(newPosition, point.Value + 1));
               checkedPositions.Remove(newPosition);
            }
         }
         
      }
         
      return tempDrisjkaMap;
      
   }

   private Dictionary<Vector2Int, int> DoTheDrisjkaMapMultiGoals()
   {

      List<Dictionary<Vector2Int, int>> maps = new List<Dictionary<Vector2Int, int>>();

      foreach (DrisjkaGoal goal in _goals)
      {
         Dictionary<Vector2Int, int> newMap = DoTheDrisjkaMapOneGoal(goal);
         if (newMap.Count > 0)
         {
            maps.Add(newMap);   
         }
         else
         {
            Debug.LogWarning("Empty Dirsjka Map !");
         }
         
      }

      if (maps.Count > 1)
      {
         Dictionary<Vector2Int, int> tempMap = new Dictionary<Vector2Int, int>();
         
         // calculate min
         foreach (KeyValuePair<Vector2Int,int> point in maps[0])
         {
            tempMap.Add(point.Key, point.Value);
            int minValue = point.Value;  
            
            foreach (Dictionary<Vector2Int,int> map in maps)
            {
               if (minValue > map[point.Key])
               {
                  minValue = map[point.Key];
                  tempMap[point.Key] = map[point.Key];
               }
            }
            
         }

         return tempMap;
      }
      else if(maps.Count == 1)
      {
         return maps[0];
      }
      else
      {
         return new Dictionary<Vector2Int, int>();
      }
      
      
   }
   
   private Dictionary<Vector2Int, int> DoTheDrisjkaMapMultiGoals_()
   {
      
      Dictionary<Vector2Int, int> tempDrisjkaMap = new Dictionary<Vector2Int, int>();

      // Check if Zeros are filled
      if (_goals.Count > 0)
      {
         // List of every positions, the list is emptied along the BFS journey
         List<Vector2Int> checkedPositions = new List<Vector2Int>();
         // First Value
         //int value = 0;

         // BFS queue : every position inspected go in the queue
         Queue<KeyValuePair<Vector2Int, int>> _tempQueuePositions = new Queue<KeyValuePair<Vector2Int, int>>();

         // Add the zeros in the queue
         foreach (DrisjkaGoal goal in _goals)
         {
            // Double check if the zeros are in the map
            if (_mapPositions.Contains(goal.Position))
            {
               // Force the priority to 0
               _tempQueuePositions.Enqueue(new KeyValuePair<Vector2Int, int>(goal.Position, goal.Priority));
               checkedPositions.Add(goal.Position);
            }
         }

         // Et zé parti
         while (_tempQueuePositions.Count > 0)
         {
            // Dequeue the last point
            KeyValuePair<Vector2Int, int> point = _tempQueuePositions.Dequeue();
            
            // Here is process  --  --  --  --  --  --  --  --  --  --  --  --  --  --  --  --  --  --
            SetValue(tempDrisjkaMap, _mapPositions, Neighbourhood.Full, point.Key, point.Value);

            // --  --  --  --  --  --  --  --  --  --  --  --  --  --  --  --  --  --  --  --  --  -- 
            foreach (var neighbour in Neighbourhood.Full)
            {
               KeyValuePair<Vector2Int, int> newPair = new KeyValuePair<Vector2Int, int>(neighbour + point.Key, point.Value);
               if (_mapPositions.Contains(newPair.Key))
               {
                  if (!checkedPositions.Contains(newPair.Key))
                  {
                     // Still in the map
                     _tempQueuePositions.Enqueue(new KeyValuePair<Vector2Int, int>(newPair.Key, newPair.Value + 1));
                     checkedPositions.Add(newPair.Key);
                  }
                  else
                  {
                     Debug.Log("Doublon !!!!!!!!!!!!!!!!");
                  }
               }
            }
            
         }
         
         // yield return null;
         
      }            
      
      return tempDrisjkaMap;
      
   }

   public List<Vector2Int> GetAPath(Vector2Int fromPosition, bool randomNeighbour)
   {

      List<Vector2Int> path = new List<Vector2Int>();
      List<Vector2Int> neighbourhood = Neighbourhood.Cardinals;
      
      Vector2Int currentPathPosition = fromPosition;
      
      // is the map filled ?
      if (_drisjkaMap.Count <= 0)
      {
         Debug.LogWarning("Empty map");
         return path;
      }
      
      // Are the positions in the drisjka map ?
      if (!_drisjkaMap.ContainsKey(fromPosition))
      {
         Debug.LogWarning("path origin not in the map " + fromPosition);
         return path;
      }

      int nbIterations = 0;
      int baseValue;
      Vector2Int basePosition;
      
      // all check
      do
      {
         baseValue = _drisjkaMap.FirstOrDefault(p => p.Key == currentPathPosition).Value;
         basePosition = currentPathPosition;
         nbIterations++;

         List<Vector2Int> actualNeighbourhood = new List<Vector2Int>(neighbourhood);
         if (randomNeighbour)
            actualNeighbourhood = neighbourhood.OrderBy(n => Random.value).ToList();
         
         foreach (Vector2Int n in actualNeighbourhood)
         {
            Vector2Int checkedPosition = currentPathPosition + n;
            
            if (_drisjkaMap.ContainsKey(checkedPosition) && !path.Contains(checkedPosition))
            {

               KeyValuePair<Vector2Int, int> point = _drisjkaMap.FirstOrDefault(p => p.Key == checkedPosition);

               if (point.Value <= baseValue)
               {
                  basePosition = checkedPosition;
                  baseValue = point.Value;
               }
            }
            
         }
         
         path.Add(basePosition);
         currentPathPosition = basePosition;

      // } while (Vector2.Distance(currentPathPosition, to) <= Mathf.Epsilon);
      } while (baseValue > _lessPriorityValue && nbIterations < _settings.MaxPathLength);
      
      return path;

   }

   private void SetValue(Dictionary<Vector2Int, int> drisjkaMap, List<Vector2Int> mapPositions, List<Vector2Int> neighbourhood, Vector2Int goalPosition, int value)
   {
      
     if(!drisjkaMap.ContainsKey(goalPosition))
      {
         // Add in dictionnary if does not exists
         Debug.Log("Add Drisjka point : " + goalPosition + "=" + value);
         drisjkaMap.Add(goalPosition, value);
      }      
      else if(drisjkaMap[goalPosition] >= value)
      {
         // Take the less value and replace
         Debug.Log("Replace Drisjka point : " + goalPosition + "=" + value);
         drisjkaMap[goalPosition] = value;
      }
      else
      {
         // Trace here
         Debug.Log("Discard Drisjka point : " + goalPosition + "=" + value);
      }
     
   }
   
}

public class DrisjkaGoal
{
   private Vector2Int _position;
   private int _priority;

   public Vector2Int Position => _position;
   public int Priority => _priority;
   
   public DrisjkaGoal(Vector2Int position, int priority)
   {
      _position = new Vector2Int(position.x, position.y);
      _priority = priority;
   }

   public override string ToString()
   {
      return "Position [" + _position + "],Priority=" + _priority;
   }
}  