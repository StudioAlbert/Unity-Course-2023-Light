using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace PCGDungeon
{
    public class TreeGraph : TreeGraphNode
    {

        public TreeGraph()
        {
            
        }
        
        // Start is called before the first frame update
        public void Initiate(String name, float ratio, BoundsSplitter.SplitDirection direction)
        {

            _name = name;
            _ratio = ratio;
            _splitDirection = direction;
            
            TreeGraphNode A  = new TreeGraphNode("A", 0.25f, PCGDungeon.BoundsSplitter.SplitDirection.Vertical);
            TreeGraphNode B  = new TreeGraphNode("B", 0.6f, PCGDungeon.BoundsSplitter.SplitDirection.Horizontal);
            TreeGraphNode Ba  = new TreeGraphNode("Ba", 0.45f, PCGDungeon.BoundsSplitter.SplitDirection.Vertical);
            TreeGraphNode Bb  = new TreeGraphNode("Bb", 0.15f, PCGDungeon.BoundsSplitter.SplitDirection.Vertical);

            Left = A;
            Right = B;
            B.Left = Ba;
            B.Right = Bb;
            
        }
        
    }
}
