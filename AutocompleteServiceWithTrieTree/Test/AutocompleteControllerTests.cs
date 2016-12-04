using AutoComplete;
using AutocompleteServiceWithTrieTree.Controllers;
using NSubstitute;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using System.Net.Http;
using System.Net;

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
            controller.Request = new HttpRequestMessage();
        }

        [Fact]
        public void Get_OkResponse()
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
        public void Get_NotFoundResponse()
        {
            var matches = new List<string>();
            client.GetPrefixMatches(Arg.Any<string>()).Returns(matches);
            NotFoundResult result = controller.GetMatches("33") as NotFoundResult;
            Assert.NotNull(result);
        }

        [Fact]
        public void Get_NullInput()
        {
            var result = controller.GetMatches(null) as BadRequestErrorMessageResult;
            Assert.NotNull(result);
            Assert.Equal<string>("Valid input is expected.", result.Message);
        }
        
        [Theory]
        [InlineData(new object[] { "a ", typeof(NotFoundResult)})]
        [InlineData(new object[] { " ar", typeof(NotFoundResult) })]
        [InlineData(new object[] { " a r", typeof(NotFoundResult) })]
        [InlineData(new object[] { "a rm", typeof(NotFoundResult) })]
        [InlineData(new object[] { "arm ", typeof(NotFoundResult) })]
        public void Get_InputWithSpaces(string prefix, Type expected)
        {
            var result = controller.GetMatches("a ");
            Assert.Equal(expected, result.GetType());
        }
        
        [Fact]
        public void Get_ExceptionHandled()
        {
            var errorMessage = "Object reference not set to an instance of an object.";
            client.When(c => c.GetPrefixMatches(Arg.Any<string>())).Throw(new NullReferenceException(errorMessage));
            BadRequestErrorMessageResult result = controller.GetMatches("ann") as BadRequestErrorMessageResult;
            Assert.NotNull(result);
            Assert.Equal<string>(errorMessage, result.Message);
        }

        [Fact]
        public void Post_Ok()
        {
            var prefix = "bee";
            client.AddItem(prefix).Returns(true);
            var result = controller.PostItem(prefix) as ResponseMessageResult;
            Assert.Equal(HttpStatusCode.Created, result.Response.StatusCode);
        }

        [Fact]
        public void Post_NoChange()
        {
            var prefix = "all";
            client.AddItem(prefix).Returns(false);
            var result = controller.PostItem(prefix) as ResponseMessageResult;
            Assert.Equal(HttpStatusCode.NotModified, result.Response.StatusCode);
        }

        [Fact]
        public void Post_NullInput()
        {
            var result = controller.PostItem(null) as BadRequestErrorMessageResult;
            Assert.NotNull(result);
            Assert.Equal<string>("Valid input is expected.", result.Message);
        }

        [Fact]
        public void Post_ExceptionHandled()
        {
            var errorMessage = "Object reference not set to an instance of an object.";
            client.When(c => c.AddItem(Arg.Any<string>())).Throw(new NullReferenceException(errorMessage));
            BadRequestErrorMessageResult result = controller.PostItem("something") as BadRequestErrorMessageResult;
            Assert.NotNull(result);
            Assert.Equal<string>(errorMessage, result.Message);
        }
    }
}
