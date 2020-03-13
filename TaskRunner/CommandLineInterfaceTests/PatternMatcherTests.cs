using CommandLineInterface;
using NUnit.Framework;

namespace Tests.CommandLineInterfaceTests
{
    public class PatternMatcherTests
    {
        [TestCase("Axxxx", "a", MatchType.Full)]
        [TestCase("AxxxxBxxx", "a", MatchType.Partial)]
        [TestCase("AxxxxBxxx", "ab", MatchType.Full)]
        [TestCase("AxxxxBxxxCxxx", "ab", MatchType.Partial)]
        [TestCase("AxxBxxCxxDxx", "abcd", MatchType.Full)]
        [TestCase("AxxBxxCd", "abcd", MatchType.Full)]
        [TestCase("AxxBcxxDxx", "abcd", MatchType.Full)]
        [TestCase("AxxBcdxx", "abcd", MatchType.Full)]
        [TestCase("AbxxCxxDxx", "abcd", MatchType.Full)]
        [TestCase("AbxxCdxx", "abcd", MatchType.Full)]
        [TestCase("AbcxxDxx", "abcd", MatchType.Full)]
        [TestCase("Abcd", "abcd", MatchType.Full)]
        [TestCase("Abcde", "abcd", MatchType.Full)]
        [TestCase("AbcdEfgh", "abcd", MatchType.Full)]
        [TestCase("AbcCde", "abcd", MatchType.Full)]
        public void Test(string @string, string pattern, MatchType expected)
        {
            var matchType = new PatternMatcher().Match(@string, pattern);
            Assert.AreEqual(expected, matchType);
        }
    }
}