namespace AwesomeCodeFixerLibrary
{
    public static class Helper
    {
        // TODO - optimize this method
        public static string TrimLines(string text, int n_first, int m_last)
        {
            var lines = text
                    .Split(Environment.NewLine.ToCharArray())
                    .Skip(n_first)
                    .Take(text.Split(Environment.NewLine.ToCharArray()).Length - n_first - m_last);
                    
            return string.Join(Environment.NewLine, lines);
        }
    }
}