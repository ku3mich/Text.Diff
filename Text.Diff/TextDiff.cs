using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Menees.Diffs;

namespace Text.Diff
{
    public class TextDiff : ITextDiff
    {
        private const string Divider = " | ";

        private static Dictionary<EditType, char> EditSymbols = new Dictionary<EditType, char> {
            { EditType.None, ' ' },
            { EditType.Delete, '-' },
            { EditType.Insert, '+' },
            { EditType.Change, '~' },
        };

        public string Generate(string a, string b, Options options = default)
            => Generate(a.SplitLines(), b.SplitLines(), options);

        public string Generate(List<string> a, List<string> b, Options options = default)
        {
            var stringDiffer = new Menees.Diffs.TextDiff(
                HashType.Unique,
                options.IgnoreWhiteSpace,
                options.IgnoreCase,
                0,
                true);

            var diffs = stringDiffer.Execute(a, b);
            var leftSideWidth = a.Max(s => (int?)s.Length) ?? 0;
            var builder = new StringBuilder();

            void AppendLine(string sa, string sb, char diffA, char diffB) =>
                      builder
                        .Append(diffA).Append(' ')
                        .Append(sa.PadRight(leftSideWidth))
                        .Append(Divider)
                        .Append(diffB).Append(' ')
                        .Append(sb)
                        .Append(StringExtensions.LF);

            var indexA = 0;
            var indexB = 0;

            Dictionary<EditType, Action<Edit>> edit = new Dictionary<EditType, Action<Edit>>
            {
                {  EditType.Delete, diff => AppendLine(a[indexA++], "", EditSymbols[diff.EditType], EditSymbols[EditType.None]) },
                {  EditType.Insert, diff => AppendLine("", b[indexB++], EditSymbols[EditType.None], EditSymbols[diff.EditType]) },
                {  EditType.Change, diff => {
                        var sA = a[indexA++];
                        var sB = b[indexB++];
                        AppendLine(sA, sB, EditSymbols[diff.EditType], EditSymbols[diff.EditType]);
                        var charDiffer = new MyersDiff<char>(sA.ToCharList(), sB.ToCharList(), true);
                        var lineDiffs = charDiffer.Execute();
                        var dA  = new StringBuilder();
                        var dB = new StringBuilder();

                        foreach (var l in lineDiffs)
                        {
                            var c = new string(EditSymbols[l.EditType], l.Length);
                            switch (l.EditType){
                                case EditType.Change:
                                    dA.Append(c.PadLeft(l.StartA -dA.Length + l.Length));
                                    dB.Append(c.PadLeft(l.StartB - dB.Length + l.Length));
                                    break;

                                case EditType.Insert:
                                    dB.Append(c.PadLeft(l.StartB - dB.Length + l.Length));
                                    break;

                                case EditType.Delete:
                                    dA.Append(c.PadLeft(l.StartA - dA.Length + l.Length));
                                    break;
                        }
                    }
                    AppendLine(dA.ToString(), dB.ToString(), EditSymbols[EditType.None], EditSymbols[EditType.None]);
                    }
                },
            };

            foreach (var diff in diffs)
            {
                while (indexA < diff.StartA || indexB < diff.StartB)
                {
                    AppendLine(a[indexA], b[indexB], EditSymbols[EditType.None], EditSymbols[EditType.None]);

                    if (indexA < diff.StartA)
                        indexA++;

                    if (indexB < diff.StartB)
                        indexB++;
                }

                for (var i = 0; i < diff.Length; i++)
                    edit[diff.EditType](diff);
            }

            while (indexA < a.Count || indexB < b.Count)
                AppendLine(a[indexA++], b[indexB++], EditSymbols[EditType.None], EditSymbols[EditType.None]);

            return builder.ToString();
        }
    }
}
