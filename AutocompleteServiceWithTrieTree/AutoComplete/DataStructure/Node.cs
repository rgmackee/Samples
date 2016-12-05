using System;
using System.Collections.Generic;

namespace AutoComplete.DataStructure
{
    /// <summary>
    /// Represents a node in the tree
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Represents a single character
        /// </summary>
        public char Value { get; }
        /// <summary>
        /// List of all children of the node. If dealing with massive number of nodes,
        /// change this to a Dictionary&lt;char, Node&gt; for constant time access
        /// </summary>
        public IList<Node> Children { get; set; }
        /// <summary>
        /// Represents the node's parent node
        /// </summary>
        public Node Parent { get; }
        /// <summary>
        /// The depth level in the tree for this node
        /// </summary>
        public int Depth { get; set; }

        public Node(char value, Node parent, int depth)
        {
            Value = value;
            Children = new List<Node>();
            Depth = depth;
            Parent = parent;
        }

        /// <summary>
        /// Attempts to find the node representing the target character
        /// </summary>
        /// <param name="value">A single character</param>
        /// <returns>A node object or null</returns>
        public Node FindChild(char value)
        {
            foreach (Node child in Children)
            {
                if (child.Value == value)
                    return child;
            }
            return null;
        }
    }
}
