using AutoComplete;
using AutocompleteServiceWithTrieTree.Controllers;
using NSubstitute;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;

namespace Test
{
    public class AutocompleteControllerTests
    {
        private IClient client;
        private AutocompleteController controller;

        public AutocompleteControllerTests()
        {
            client = Substitute.For<IClient>();
            controller = new AutocompleteController(client);
        }

        [Fact]
        public void GetOkResponse()
        {
            string prefix = "ab";
            var matches = new List<string> { "abe", "aba" };
            client.GetPrefixMatches(prefix).Returns(matches);
            var result = controller.GetMatches(prefix) as OkNegotiatedContentResult<List<string>>;
            var list = result.Content as List<string>;
            Assert.Equal<int>(2, list.Count);
            Assert.Equal<string>("aba", list.First()); //sorted
        }

        [Fact]
        public void GetNotFoundResponse()
        {
            var matches = new List<string>();
            client.GetPrefixMatches(Arg.Any<string>()).Returns(matches);
            NotFoundResult result = controller.GetMatches("33") as NotFoundResult;
            Assert.NotNull(result);
        }
        
        [Fact]
        public void ExceptionHandled()
        {
            var errorMessage = "Object reference not set to an instance of an object.";
            client.When(c => c.GetPrefixMatches(Arg.Any<string>())).Throw(new NullReferenceException(errorMessage));
            BadRequestErrorMessageResult result = controller.GetMatches(Arg.Any<string>()) as BadRequestErrorMessageResult;
            Assert.NotNull(result);
            Assert.Equal<string>(errorMessage, result.Message);
        }
    }
}
