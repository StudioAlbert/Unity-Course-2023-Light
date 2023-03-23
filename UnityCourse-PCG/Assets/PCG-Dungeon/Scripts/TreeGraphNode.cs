using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCGDungeon
{
    public class TreeGraphNode
    {

        protected BoundsSplitter.SplitDirection _splitDirection = BoundsSplitter.SplitDirection.None;
        protected String _name;
        protected float _ratio;

        private TreeGraphNode _left;
        public TreeGraphNode Left { get { return _left; } set { _left = value; } }

        private TreeGraphNode _right;
        public TreeGraphNode Right { get { return _right; } set { _right = value; } }


        public TreeGraphNode(String name, float ratio,BoundsSplitter. SplitDirection direction)
        {
            _name = name;
            _ratio = ratio;
            _splitDirection = direction;
        }
        public TreeGraphNode()
        {
        }

        public void Process(BoundsInt mapToProcess, List<BoundsInt> mapsToAdd)
        {

            if (_ratio >= 0 && _ratio <= 1 && _splitDirection != BoundsSplitter.SplitDirection.None)
            {
                // BoundsInt mapLeft = new BoundsInt();
                // BoundsInt mapRight = new BoundsInt();

                BoundsSplitter.SplitBounds(mapToProcess, _ratio, _splitDirection, out BoundsInt mapLeft, out BoundsInt mapRight);
                
                if (_left != null)
                    _left.Process(mapLeft, mapsToAdd);
                else
                    mapsToAdd.Add(mapLeft);

                if (_right != null)
                    _right.Process(mapRight, mapsToAdd);
                else
                    mapsToAdd.Add(mapRight);

            }
            
        }

    }

}
