using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG
{
   [ExecuteInEditMode]
    public class GraphLink
    {
        
        private GraphNode _from;
        private GraphNode _to;

        public GraphLink(GraphNode from, GraphNode to)
        {
            _from = from;
            _to = to;
        }
        
        // Update is called once per frame
        public void Draw()
        {
            if (_from != null && _to != null)
                Debug.DrawLine(_from.transform.position, _to.transform.position, Color.blue);
        }
    }
}
