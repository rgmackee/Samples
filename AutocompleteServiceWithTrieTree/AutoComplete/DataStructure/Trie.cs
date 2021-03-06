﻿using System;
using System.Linq;
using System.Collections.Generic;

namespace AutoComplete.DataStructure
{
    /// <summary>
    /// This class represents the Trie tree data structure. 
    /// Only the methods needed for this example are implemented.
    /// </summary>
    public class Trie
    {
        private readonly Node root;

        public Trie()
        {
            //Use the '^' character to mark the root node
            this.root = new Node('^', parent: null, depth: 0);
        }

        /// <summary>
        /// Adds all items in the source enumerable
        /// </summary>
        /// <param name="items">Collection of items</param>
        public void AddRange(IEnumerable<string> items)
        {
            foreach (string s in items)
            {
                Add(s.ToLower());
            }
        }

        /// <summary>
        /// Adds an individual item to the Trie
        /// </summary>
        /// <param name="s">String to be added</param>
        /// <returns></returns>
        public Node Add(string s)
        {
            //find the pivot character where to branch out for a different suffix. Root returned if first letter not found
            var commonPrefix = FindCommonPrefixNode(s, true);
            if (commonPrefix.Depth == s.Length && commonPrefix.Children.FirstOrDefault(n => n.Value == '$') != null)
            {
                //This word already exists as complete
                return null;
            }
            var current = commonPrefix;
            for (var i = current.Depth; i < s.Length; i++)
            {
                var node = new Node(s[i], current, current.Depth + 1);
                current.Children.Add(node);
                current = node;
            }
            //word is complete, mark it as such
            current.Children.Add(new Node('$', current, current.Depth + 1));
            return current;
        }

        /// <summary>
        /// Finds all matches from a prefix
        /// </summary>
        /// <param name="input">Prefix to be searched</param>
        /// <returns></returns>
        public IEnumerable<string> FindMatches(string input)
        {
            if (!String.IsNullOrWhiteSpace(input))
            {
                var node = FindCommonPrefixNode(input);
                if (node != null)
                {
                    Stack<List<char>> stack = new Stack<List<char>>();
                    foreach (Node child in node.Children)
                    {
                        stack.Push(new List<char>(input.ToCharArray()));
                        TraverseSubtree(child, stack);
                    }
                    foreach (var word in stack)
                    {
                        yield return new String(word.ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// Finds a node from where to diverge, if there is a common prefix.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isAdding"></param>
        /// <returns>Node of the last matching character or the Root</returns>
        internal Node FindCommonPrefixNode(string input, bool isAdding = false)
        {
            var currentNode = root;
            var node = currentNode;
            foreach (char c in input)
            {
                currentNode = currentNode.FindChild(c);
                if (currentNode == null)
                {
                    if (!isAdding)
                    {
                        //a match for a prefix was not found
                        return null;
                    }
                    else
                    {
                        //the current node is the point where a different word would be added
                        break;
                    }
                }
                node = currentNode;
            }
            return node;
        }

        private void TraverseSubtree(Node node, Stack<List<char>> stack)
        {
            List<char> current = stack.Pop();
            if (node.Children.Count > 0)
            {
                foreach (Node child in node.Children)
                {
                    List<char> appended = new List<char>(current);
                    appended.Add(node.Value);
                    stack.Push(appended);
                    TraverseSubtree(child, stack);
                }
            }
            else if (node.Value == '$')
            {
                stack.Push(current);
            }
        }
    }
}
