using AutoComplete;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Web.Http;
using System.Net;
using System.Web.Http.Results;

namespace AutocompleteServiceWithTrieTree.Controllers
{
    /// <summary>
    /// Provides endpoints to get autocomplete matches and add items to the list in memory.
    /// </summary>
    public class AutocompleteController : ApiController
    {
        private IClient dataClient;

        public AutocompleteController(IClient dataClient)
        {
            this.dataClient = dataClient;
        }

        /// <summary>
        /// Provides a list of matches to a prefix
        /// </summary>
        /// <param name="value">Prefix to be searched</param>
        /// <returns>Array of matches if successful</returns>
        public IHttpActionResult GetMatches(string value = null)
        {
            if (value == null)
                return BadRequest("Valid input is expected.");
            try
            {
                string validated = ValidateInput(value);
                if (validated != null)
                {
                    var list = dataClient.GetPrefixMatches(validated).ToList();
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
        
        /// <summary>
        /// Adds an item to the tree in memory only.
        /// </summary>
        /// <param name="value">Item to be added</param>
        /// <returns></returns>
        public IHttpActionResult PostItem([FromUri]string value = null)
        {
            if (value == null)
                return BadRequest("Valid input is expected.");
            try
            {
                string validated = ValidateInput(value);
                if (validated != null)
                {
                    if (dataClient.AddItem(validated))
                        return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.Created));
                }
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotModified));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string ValidateInput(string input)
        {
            //input = input.TrimEnd();
            //only take letters for this sample
            if (Regex.IsMatch(input, "^[a-z]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase))
            {
                return input.ToLower();
            }
            return null;
        }
    }
}