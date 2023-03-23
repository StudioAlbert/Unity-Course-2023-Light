using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG
{
    public class TreeNode : MonoBehaviour
    {

        private String _name;
        private List<TreeNode> _children = new List<TreeNode>();
        public List<TreeNode> Children => _children;
        
        [SerializeField] Renderer _markTarget;
        private Color _originalColor;

        private void Start()
        {
            _name = name;
            _originalColor = _markTarget.material.color;

        }

        public void AddChild(TreeNode node)
        {
            if(!_children.Contains(node))
                _children.Add(node);
        }

        public override string ToString()
        {
            return _name;
        }
        public void Mark()
        {
            Renderer _markTarget = this.gameObject.GetComponentInChildren<Renderer>();
            _markTarget.material.color = Color.yellow;
        }
        public void Unmark()
        {
            _markTarget.material.color = _originalColor;
        }
    }
}
