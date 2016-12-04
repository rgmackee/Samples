using AutoComplete;
using AutocompleteServiceWithTrieTree.Models;
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

        public IHttpActionResult GetMatches(string value)
        {
            try
            {
                value = value.Trim().ToLower();
                //only take letters for this sample
                if (Regex.IsMatch(value, "^[a-z]+$", RegexOptions.Compiled))
                {
                    var list = dataClient.GetPrefixMatches(value).ToList();
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
        
        [HttpPost]
        public IHttpActionResult PostItem(string value)
        {
            try
            {
                value = value.Trim().ToLower();
                bool added = dataClient.AddItem(value);
                return Ok(new PostResult { Success = added });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}