using System;
using AutoComplete.DataStructure;
using System.Collections.Generic;

namespace AutoComplete
{
    public class Client : IClient
    {
        private Trie tree;

        public Client(IEnumerable<string> data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            tree = new Trie();
            tree.AddRange(data);
        }

        /// <summary>
        /// Adds an item to the trie
        /// </summary>
        /// <param name="value">Value to be added</param>
        /// <returns></returns>
        public bool AddItem(string value)
        {
            var node = tree.Add(value);
            return node != null;
        }

        /// <summary>
        /// Gets all items that match the prefix
        /// </summary>
        /// <param name="prefix">Prefix to be searched</param>
        /// <returns>List of matches</returns>
        public IEnumerable<string> GetPrefixMatches(string prefix)
        {
            IEnumerable<string> list = tree?.FindMatches(prefix);
            return list;
        }
    }
}
