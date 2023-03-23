using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG
{
    public class Tree : MonoBehaviour
    {

        [SerializeField] private List<TreeNode> _availableNodes = new List<TreeNode>();
        [SerializeField] private TreeNode _root = new TreeNode();
        
        // Start is called before the first frame update
        void Start()
        {
            // TreeNode newNode = _availableNodes.Find(n => n.name == "Point 009");
            // if(newNode != null)
            //     _root = newNode;
            TreeNode branchA = GetnodeByName("Point 004");
            TreeNode branchB = GetnodeByName("Point 003");
            TreeNode branchBb = GetnodeByName("Point 008");
            
            branchA.AddChild(GetnodeByName("Point 007"));
            branchA.AddChild(GetnodeByName("Point 002"));
            
            branchBb.AddChild(GetnodeByName("Point 006"));
            branchBb.AddChild(GetnodeByName("Point 010"));
            
            branchB.AddChild(GetnodeByName("Point 005"));
            branchB.AddChild(GetnodeByName("Point 001"));
            branchB.AddChild(branchBb);
            
            _root.AddChild(branchA);
            _root.AddChild(branchB);

            // StartCoroutine(BFSOrder());
            // StartCoroutine(DFSOrder());

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void VisitInBFSOrder()
        {
            foreach (TreeNode nodeToUnMark in _availableNodes)
            {
                nodeToUnMark.Unmark();
            }
            
            StopAllCoroutines();
            StartCoroutine(BFSOrder());
        }        
        
        public void VisitInDFSOrder()
        {
            foreach (TreeNode nodeToUnMark in _availableNodes)
            {
                nodeToUnMark.Unmark();
            }
            
            StopAllCoroutines();
            StartCoroutine(DFSOrder());
        }
        
        IEnumerator BFSOrder()
        {

            Queue<TreeNode> _tempQueue = new Queue<TreeNode>();
            String treeTrace = "";
            
            _tempQueue.Enqueue(_root);

            do
            {
                TreeNode tn = _tempQueue.Dequeue();
                
                // Here is process -------------------------------------
                treeTrace += tn.ToString() + "\n";
                tn.Mark();
                yield return new WaitForSeconds(1f);
                
                foreach (var child in tn.Children)
                    _tempQueue.Enqueue(child);

            } while (_tempQueue.Count > 0);

            Debug.Log(treeTrace);
            yield return null;

        }
        
        IEnumerator DFSOrder()
        {

            Stack<TreeNode> _tempQueue = new Stack<TreeNode>();
            List<TreeNode> _visited = new List<TreeNode>();
            String treeTrace = "";
            
            _tempQueue.Push(_root);

            do
            {
                TreeNode tn = _tempQueue.Pop();
                _visited.Add(tn);
                
                // Here is process -------------------------------------
                treeTrace += tn.ToString() + "\n";
                tn.Mark();
                yield return new WaitForSeconds(1f);

                foreach (var child in tn.Children)
                {
                    if (!_visited.Contains(child))
                    {
                        _tempQueue.Push(child);    
                    }
                }

            } while (_tempQueue.Count > 0);

            Debug.Log(treeTrace);
            yield return null;

        }
        
        private TreeNode GetnodeByName(String name)
        {

            TreeNode newNode;
            
            newNode = _availableNodes.Find(n => n.name == name);
            if (newNode != null)
                return newNode;
            else
                return new TreeNode();
            
        }
    }
}
