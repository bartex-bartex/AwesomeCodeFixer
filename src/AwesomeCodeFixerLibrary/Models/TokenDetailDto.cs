using System.Drawing;

namespace AwesomeCodeFixerLibrary;

public class TokenDetailDto : IComparable<TokenDetailDto>
{
    public string Token { get; set; }
    public Point StartPosition { get; set; }
    public Point EndPosition { get; set; }

    /// <summary>
    /// Specify position of token key in parrent component before tokens are replaced.
    /// </summary>
    public Point RelativeStartPosition { get; set;}

    public TokenDetailDto(string token, Point startPosition, Point endPosition)
    {
        Token = token;
        StartPosition = startPosition;
        EndPosition = endPosition;
    }

    public int CompareTo(TokenDetailDto? other)
    {
        if (other == null) return 1;

        if (StartPosition.X < other.StartPosition.X) return -1;
        if (StartPosition.X > other.StartPosition.X) return 1;

        if (StartPosition.Y < other.StartPosition.Y) return -1;
        if (StartPosition.Y > other.StartPosition.Y) return 1;

        return 0;
    }
}