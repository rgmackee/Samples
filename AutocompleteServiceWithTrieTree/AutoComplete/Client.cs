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

        public bool AddItem(string value)
        {
            var node = tree.Add(value);
            return node != null;
        }

        public IEnumerable<string> GetPrefixMatches(string prefix)
        {
            IEnumerable<string> list = tree?.FindMatches(prefix);
            return list;
        }
    }
}
