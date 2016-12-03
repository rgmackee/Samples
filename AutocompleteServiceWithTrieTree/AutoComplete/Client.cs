using System;
using AutoComplete.DataStructure;
using System.Collections.Generic;

namespace AutoComplete
{
    public class Client : IClient
    {
        private readonly IEnumerable<string> data;
        private Trie tree;

        public Client(IEnumerable<string> data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            tree = new Trie();
            tree.AddRange(data);
        }

        public IEnumerable<string> GetPrefixMatches(string prefix)
        {
            IEnumerable<string> list = tree?.FindMatches(prefix);
            return list;
        }
    }
}
