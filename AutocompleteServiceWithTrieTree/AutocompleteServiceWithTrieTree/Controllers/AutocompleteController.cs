using AutoComplete;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace AutocompleteServiceWithTrieTree.Controllers
{
    public class AutocompleteController : ApiController
    {
        private IClient dataClient;

        public AutocompleteController(IClient dataClient)
        {
            this.dataClient = dataClient;
        }

        public IHttpActionResult GetMatches(string prefix)
        {
            try
            {
                prefix = prefix.ToLower();
                //only take letters for this sample
                if (Regex.IsMatch(prefix, "^[a-z]+$", RegexOptions.Compiled))
                {
                    var list = dataClient.GetPrefixMatches(prefix).ToList();
                    if (list.Any())
                    {
                        list.Sort();
                        return Ok(list);
                    }
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}