using AutoComplete;
using AutocompleteServiceWithTrieTree.Controllers;
using NSubstitute;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using AutocompleteServiceWithTrieTree.Models;

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
        public void Get_ExceptionHandled()
        {
            var errorMessage = "Object reference not set to an instance of an object.";
            client.When(c => c.GetPrefixMatches(Arg.Any<string>())).Throw(new NullReferenceException(errorMessage));
            BadRequestErrorMessageResult result = controller.GetMatches(Arg.Any<string>()) as BadRequestErrorMessageResult;
            Assert.NotNull(result);
            Assert.Equal<string>(errorMessage, result.Message);
        }

        [Fact]
        public void Post_Ok()
        {
            var prefix = "bee";
            client.AddItem(prefix).Returns(true);
            var result = controller.PostItem(prefix) as OkNegotiatedContentResult<PostResult>;
            var post = result.Content as PostResult;
            Assert.NotNull(post);
            Assert.Equal<bool>(true, post.Success);
        }

        [Fact]
        public void Post_NoChange()
        {
            var prefix = "all";
            client.AddItem(prefix).Returns(false);
            var result = controller.PostItem(prefix) as OkNegotiatedContentResult<PostResult>;
            var post = result.Content as PostResult;
            Assert.NotNull(post);
            Assert.Equal<bool>(false, post.Success);
        }

        [Fact]
        public void Post_ExceptionHandled()
        {
            var errorMessage = "Object reference not set to an instance of an object.";
            client.When(c => c.AddItem(Arg.Any<string>())).Throw(new NullReferenceException(errorMessage));
            BadRequestErrorMessageResult result = controller.GetMatches(Arg.Any<string>()) as BadRequestErrorMessageResult;
            Assert.NotNull(result);
            Assert.Equal<string>(errorMessage, result.Message);
        }
    }
}
