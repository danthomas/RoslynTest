using System;
using System.Collections.Generic;
using System.Linq;
using CommandLineInterface;
using NUnit.Framework;

namespace Tests.CommandLineInterfaceTests
{
    public class PatternGeneratorTests
    {
        [Test]
        public void Test4Chars()
        {
            var pattern = "ABCD";

            var patterns = new PatternGenerator().Generate(pattern);

            var actual = Environment.NewLine + string.Join(Environment.NewLine, patterns.Select(x => string.Join((string?)", ", (IEnumerable<string?>)x)));

            var expected = @"
A, B, C, D
A, B, CD
A, BC, D
A, BCD
AB, C, D
AB, CD
ABC, D
ABCD";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test5Chars()
        {
            var pattern = "ABCDE";

            var patterns = new PatternGenerator().Generate(pattern);

            var actual = Environment.NewLine + string.Join(Environment.NewLine, patterns.Select(x => string.Join((string?)", ", (IEnumerable<string?>)x)));

            var expected = @"
A, B, C, D, E
A, B, C, DE
A, B, CD, E
A, B, CDE
A, BC, D, E
A, BC, DE
A, BCD, E
A, BCDE
AB, C, D, E
AB, C, DE
AB, CD, E
AB, CDE
ABC, D, E
ABC, DE
ABCD, E
ABCDE";
            Assert.AreEqual(expected, actual);
        }
    }
}