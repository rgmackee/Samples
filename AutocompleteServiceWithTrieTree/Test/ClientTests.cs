using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using AutoComplete;

namespace Test
{
    public class ClientTests
    {
        private Client client;
        private string[] data = { "arm", "armed", "arms", "all", "dam", "damn", "jazz", "jaws" };

        public ClientTests()
        {
            client = new Client(data);
        }

        [Theory]
        [InlineData("al", new string[] { "all" })]
        [InlineData("arm", new string[] { "arm", "armed", "arms" })]
        [InlineData("d", new string[] { "dam", "damn" })]
        [InlineData("ja", new string[] { "jaws", "jazz" })]
        [InlineData("jaws", new string[] { "jaws" })]
        public void PositiveMatches(string prefix, IEnumerable<string> expected)
        {
            var list = client.GetPrefixMatches(prefix)?.ToArray();
            if (list?.Length > 1)
                Array.Sort<string>(list);
            Assert.Equal(expected, list);
        }

        [Theory]
        [InlineData("ab", new string[0])]
        [InlineData("are", new string[0])]
        [InlineData("x", new string[0])]
        [InlineData("jac", new string[0])]
        [InlineData("dar", new string[0])]
        public void NegativeMatches(string prefix, IEnumerable<string> expected)
        {
            var list = client.GetPrefixMatches(prefix)?.ToArray();
            if (list?.Length > 1)
                Array.Sort<string>(list);
            Assert.Equal(expected, list);
        }

        [Fact]
        public void EmptyInput()
        {
            var list = client.GetPrefixMatches("")?.ToArray();
            Assert.Equal(new string[0], list);
        }

        [Fact]
        public void NullInput()
        {
            var list = client.GetPrefixMatches(null)?.ToArray();
            Assert.Equal(new string[0], list);
        }

        [Fact]
        public void NullDataToClient()
        {
            Assert.Throws<ArgumentNullException>(() => new Client(null));
        }

        [Fact]
        public void AddNewItemToTree()
        {
            bool added = client.AddItem("ale");
            Assert.Equal<bool>(true, added);
        }

        [Fact]
        public void DoNotAddExistingItem()
        {
            bool added = client.AddItem("arm");
            Assert.Equal<bool>(false, added);
        }
    }
}
