using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace Text.Diff
{
    static class StringExtensions
    {
        [return: NotNull()]
        public static IEnumerable<string> Split([NotNull] this IEnumerable<char> chars, char divider)
        {
            var t = new StringBuilder();
            foreach (var c in chars)
            {
                if (c == divider)
                {
                    yield return t.ToString();
                    t.Clear();
                    continue;
                }
                t.Append(c);
            }
            yield return t.ToString();
        }

        [return: NotNull()]
        public static List<string> SplitLines([MaybeNull] this string s)
            => s?.Where(q => q != CR).Split(LF).ToList() ?? new List<string>();

        [return: NotNull()]
        public static List<char> ToCharList([NotNull] this string s)
            => new List<char>(s.ToCharArray());

        public const char LF = '\n';
        public const char CR = '\r';
    }
}
