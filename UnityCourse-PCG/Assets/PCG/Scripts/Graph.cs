using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace PCG
{
     [ExecuteInEditMode]
     public class Graph : MonoBehaviour
    {

        public List<GraphNode> AvailableNodes
        {
            get
            {
                _availableNodes = FindObjectsOfType<GraphNode>().ToList();
                return _availableNodes;
            }
        }
        private List<GraphNode> _availableNodes = new List<GraphNode>();
        
        private List<GraphLink> _links = new List<GraphLink>();

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            foreach (GraphLink link in _links)
            {
                link.Draw();
            }
        }

        public void AddLink(GraphNode from, GraphNode to)
        {
            if (from != null && to != null)
            {
                Debug.Log("add a node from : " + from + " to " + to);
                _links.Add(new GraphLink(from, to));
            }
        }

        public void ClearLinks()
        {
            _links.Clear();
        }
    }
}
