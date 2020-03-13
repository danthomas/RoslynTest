using System.Linq;

namespace CommandLineInterface
{
    public static class StringExtensions
    {
        public static string[] ToWords(this string thisString)
        {
            if (string.IsNullOrWhiteSpace(thisString))
            {
                return new string[0];
            }

            var enumerable = thisString
                .ToCharArray()
                .Select(x => (char.IsUpper(x) ? " " : "") + x).ToList();

            return string.Join("", enumerable)
                .Trim()
                .Split(" ");
        }

        public static string ToAcronym(this string thisString)
        {
            return string.IsNullOrWhiteSpace(thisString)
                ? ""
                : new string(thisString.ToCharArray().Where(char.IsUpper).ToArray());
        }
    }
}