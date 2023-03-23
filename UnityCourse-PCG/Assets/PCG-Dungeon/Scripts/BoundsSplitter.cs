using System.Collections.Generic;
using UnityEngine;

namespace PCGDungeon
{
    public static class BoundsSplitter
    {
        public enum SplitDirection
        {
            Horizontal,
            Vertical,
            None
        }

        public static void SplitBounds(BoundsInt mapToProcess, float ratio, SplitDirection direction, out BoundsInt mapLeft, out BoundsInt mapRight)
        {

            mapLeft = new BoundsInt();
            mapRight = new BoundsInt();
            
            if (direction == SplitDirection.Horizontal)
            {
                HorizonSplitBounds(mapToProcess, ratio, out mapLeft, out mapRight);
            }else if (direction == SplitDirection.Vertical)
            {
                VerticalSplitBounds(mapToProcess, ratio, out mapLeft, out mapRight);
            }
            
        }

        private static void HorizonSplitBounds(BoundsInt mapToProcess, float ratio, out BoundsInt mapLeft, out BoundsInt mapRight)
        {
            mapLeft = new BoundsInt();
            mapLeft.xMin = mapToProcess.xMin;
            mapLeft.xMax = mapToProcess.xMax;
            mapLeft.yMin = mapToProcess.yMin;
            mapLeft.yMax = mapToProcess.yMin + Mathf.FloorToInt(ratio * mapToProcess.size.y);
            
            mapRight = new BoundsInt();
            mapRight.xMin = mapToProcess.xMin;
            mapRight.xMax = mapToProcess.xMax;
            mapRight.yMin = mapLeft.yMax;
            mapRight.yMax = mapToProcess.yMax;
            
        }

        private static void VerticalSplitBounds(BoundsInt mapToProcess, float ratio, out BoundsInt mapLeft, out BoundsInt mapRight)
        {
            mapLeft = new BoundsInt();
            mapLeft.xMin = mapToProcess.xMin;
            mapLeft.xMax = mapToProcess.xMin + Mathf.FloorToInt(ratio * mapToProcess.size.x);
            mapLeft.yMin = mapToProcess.yMin;
            mapLeft.yMax = mapToProcess.yMax;

            mapRight = new BoundsInt();
            mapRight.xMin = mapLeft.xMax;
            mapRight.xMax = mapToProcess.xMax;
            mapRight.yMin = mapToProcess.yMin;
            mapRight.yMax = mapToProcess.yMax;
        }
        
        public static void DrawDebug(List<BoundsInt> rooms, Vector3 shrinkRatio)
        {
            foreach (BoundsInt room in rooms)
            {
                DebugDrawBounds(room, Color.blue);
                DebugDrawBounds(ShrinkedRoom(room, shrinkRatio), Color.yellow);
            }
            
        }
        
        private static void DebugDrawBounds(BoundsInt debugRoom, Color color)
        {
            
            Vector3 pointA = new Vector3(debugRoom.xMin, debugRoom.yMin, debugRoom.zMin);
            Vector3 pointB = new Vector3(debugRoom.xMax, debugRoom.yMin, debugRoom.zMin);
            Vector3 pointC = new Vector3(debugRoom.xMin, debugRoom.yMax, debugRoom.zMin);
            Vector3 pointD = new Vector3(debugRoom.xMax, debugRoom.yMax, debugRoom.zMin);
            Vector3 pointE = new Vector3(debugRoom.xMin, debugRoom.yMin, debugRoom.zMax);
            Vector3 pointF = new Vector3(debugRoom.xMax, debugRoom.yMin, debugRoom.zMax);
            Vector3 pointG = new Vector3(debugRoom.xMin, debugRoom.yMax, debugRoom.zMax);
            Vector3 pointH = new Vector3(debugRoom.xMax, debugRoom.yMax, debugRoom.zMax);
            
            Debug.DrawLine(pointA, pointB, color);
            Debug.DrawLine(pointB, pointD, color);
            Debug.DrawLine(pointD, pointC, color);
            Debug.DrawLine(pointC, pointA, color);

            if (debugRoom.zMin != debugRoom.zMax)
            {
                Debug.DrawLine(pointE, pointF, color);
                Debug.DrawLine(pointF, pointH, color);
                Debug.DrawLine(pointH, pointG, color);
                Debug.DrawLine(pointG, pointE, color);
                
                Debug.DrawLine(pointC, pointG, color);
                Debug.DrawLine(pointD, pointH, color);
                Debug.DrawLine(pointA, pointE, color);
                Debug.DrawLine(pointB, pointF, color);
                
            }
            
        }
        
        public static BoundsInt ShrinkedRoom(BoundsInt room, Vector3 shrinkRatio)
        {

            BoundsInt shrinkedRoom = room;
            // ------------------------------------------------------------------------------------------------------------------
            
            shrinkedRoom.xMin = room.xMin + Mathf.CeilToInt(0.5f * (1f - shrinkRatio.x) * room.size.x);
            shrinkedRoom.xMax = room.xMax - Mathf.CeilToInt(0.5f * (1f - shrinkRatio.x) * room.size.x);
            // ------------
            
            shrinkedRoom.yMin = room.yMin + Mathf.CeilToInt(0.5f * (1f - shrinkRatio.y) * room.size.y);
            shrinkedRoom.yMax = room.yMax - Mathf.CeilToInt(0.5f * (1f - shrinkRatio.y) * room.size.y);
            // ------------
            
            shrinkedRoom.zMin = room.zMin + Mathf.CeilToInt(0.5f * (1f - shrinkRatio.z) * room.size.z);
            shrinkedRoom.zMax = room.zMax - Mathf.CeilToInt(0.5f * (1f - shrinkRatio.z) * room.size.z);
            // ------------
            
            return shrinkedRoom;
        }


    }
}
