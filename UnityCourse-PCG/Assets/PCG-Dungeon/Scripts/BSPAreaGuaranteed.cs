using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PCGDungeon
{
    public class BSPAreaGuaranteed
    {

        public enum DirectionPickerMode
        {
            Random,
            Toggle,
            AmongBounds
        }
        
        public BSPAreaGuaranteed()
        {

        }

        public void Process(BoundsInt mapToProcess, List<BoundsInt> roomsList, DirectionPickerMode directionPickerMode, float minWidth, float minHeight, float minRatio,
            float maxRatio, float xyDifference)
        {

            Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
            BoundsSplitter.SplitDirection direction = BoundsSplitter.SplitDirection.Horizontal;
            if (Random.value > 0.5f)
                // Horizontal
                direction = BoundsSplitter.SplitDirection.Horizontal;
            else
                // Vertical
                direction = BoundsSplitter.SplitDirection.Vertical;

            roomsQueue.Enqueue(mapToProcess);

            while (roomsQueue.Count > 0)
            {
                var room = roomsQueue.Dequeue();

                switch (directionPickerMode)
                {
                    case DirectionPickerMode.Random:
                        direction = RandomDirection();
                        break;
                    case DirectionPickerMode.Toggle:
                        direction = ToggleDirection(direction);
                        break;
                    case DirectionPickerMode.AmongBounds: 
                        direction = CutAmongBounds(room, xyDifference);
                        break;
                }

                if (room.size.x < minWidth || room.size.y < minHeight)
                {
                    roomsList.Add(room);
                }
                else
                {
                    // BoundsInt room1 = new BoundsInt();
                    // BoundsInt room2 = new BoundsInt();

                    BoundsSplitter.SplitBounds(room,
                        Random.Range(Mathf.Max(Mathf.Epsilon, minRatio), Mathf.Min(maxRatio, 1f)), direction, out BoundsInt room1, out BoundsInt room2);
                    roomsQueue.Enqueue(room1);
                    roomsQueue.Enqueue(room2);

                }

            }
        }

        private BoundsSplitter.SplitDirection CutAmongBounds(BoundsInt room, float difference)
        {
            if (Mathf.Abs(room.size.x - room.size.y) < difference)
                return RandomDirection();
            
            if (room.size.x > room.size.y)
                return BoundsSplitter.SplitDirection.Vertical;
            else
                return BoundsSplitter.SplitDirection.Horizontal;
            
        }

        private static BoundsSplitter.SplitDirection RandomDirection()
        { 
            if (Random.value > 0.5f)
                // Horizontal
                return BoundsSplitter.SplitDirection.Horizontal;
            else
                // Vertical
                return BoundsSplitter.SplitDirection.Vertical;
            
        }

        private static BoundsSplitter.SplitDirection ToggleDirection(BoundsSplitter.SplitDirection direction)
        {
            BoundsSplitter.SplitDirection newDirection;
            if (direction == BoundsSplitter.SplitDirection.Horizontal)
                newDirection = BoundsSplitter.SplitDirection.Vertical;
            else
                newDirection = BoundsSplitter.SplitDirection.Horizontal;

            return newDirection;
            
        }
    }
}