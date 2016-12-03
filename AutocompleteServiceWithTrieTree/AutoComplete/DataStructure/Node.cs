using System;
using System.Collections.Generic;

namespace AutoComplete.DataStructure
{
    public class Node
    {
        public char Value { get; }
        public IList<Node> Children { get; set; }
        public Node Parent { get; }
        public int Depth { get; set; }

        public Node(char value, Node parent, int depth)
        {
            Value = value;
            Children = new List<Node>();
            Depth = depth;
            Parent = parent;
        }

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
