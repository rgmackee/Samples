using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoComplete
{
    public interface IClient
    {
        IEnumerable<string> GetPrefixMatches(string prefix);
    }
}
