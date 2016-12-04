using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoComplete
{
    public interface IClient
    {
        /// <summary>
        /// Gets all items that match the prefix
        /// </summary>
        /// <param name="prefix">Prefix to be searched</param>
        /// <returns>List of matches</returns>
        IEnumerable<string> GetPrefixMatches(string prefix);
        /// <summary>
        /// Adds an item to the trie
        /// </summary>
        /// <param name="value">Value to be added</param>
        /// <returns></returns>
        bool AddItem(string value);
    }
}
