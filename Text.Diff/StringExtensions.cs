using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using Menees;

namespace Text.Diff
{
    static class StringExtensions
    {
        [return: NotNull()]
        public static List<string> SplitLines([MaybeNull] this string s)
        {
            if (s == null || s.IsEmpty())
                return new List<string>();

            var sb = new StringBuilder();

            for (var i = 0; i < s.Length; i++)
            {
                if (s[i] != CR && s[i] != LF)
                {
                    sb.Append(s[i]);
                    continue;
                }

                if (s[i] == LF)
                {
                    sb.Append("\\n\n");
                    continue;
                }

                if (s[i] == CR)
                {
                    sb.Append("\\r");
                    if (s[i + 1] == LF)
                    {
                        i++;
                        sb.Append("\\n");
                    }
                    sb.Append(LF);
                }
            }

            return sb.ToString().Split(LF).ToList();
        }

        [return: NotNull()]
        public static List<char> ToCharList([NotNull] this string s)
            => new List<char>(s.ToCharArray());

        public const char LF = '\n';
        public const char CR = '\r';
    }
}
