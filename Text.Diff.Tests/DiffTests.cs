using Xunit;
using Xunit.Abstractions;

namespace Text.Diff.Tests
{
    public class DiffTests
    {
        readonly TextDiff Diff = new TextDiff();
        private readonly ITestOutputHelper Console;

        public DiffTests(ITestOutputHelper console)
        {
            Console = console;
        }

        [Theory]
        [InlineData(null, null, "")]
        [InlineData("", null, "-  |   \n")]
        [InlineData(null, "", "   | + \n")]
        public void AcceptNulls(string a, string b, string expected)
        {
            var result = Diff.Generate(a, b);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("asdf", "asdf1", "~ asdf | ~ asdf1\n       |       +\n")]
        [InlineData("asdf", "asd", "~ asdf | ~ asd\n     - |   \n")]
        [InlineData("qwe", "1", "~ qwe | ~ 1\n  ~-- |   ~\n")]
        public void LineChange(string a, string b, string e)
        {
            var result = Diff.Generate(a, b);
            Console.WriteLine(result);
            Assert.Equal(e, result);
        }
    }
}
