namespace Text.Diff
{
    public readonly struct Options
    {
        public readonly bool IgnoreWhiteSpace;
        public readonly bool IgnoreCase;

        public Options(bool ignoreWhiteSpace, bool ignoreCase)
        {
            IgnoreWhiteSpace = ignoreWhiteSpace;
            IgnoreCase = ignoreCase;
        }
    }
}
