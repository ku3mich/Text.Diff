﻿using System.Collections.Generic;

namespace Text.Diff
{
    public interface ITextDiff
    {
        string Generate(string a, string b, Options options = default);
        string Generate(List<string> a, List<string> b, Options options = default);
    }
}
