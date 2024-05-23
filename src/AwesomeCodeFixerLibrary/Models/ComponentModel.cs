using System.Drawing;

namespace AwesomeCodeFixerLibrary; 

public class ComponentModel
{
    public string Token { get; set; }
    public string Content { get; set; }
    public ComponentType ComponentType { get; set; }
    public Point StartPosition { get; set; }
    public Point EndPosition { get; set; }
    public List<ErrorModel> Errors { get; set; }
    public List<ComponentModel> Children { get; set; } = new();

    public ComponentModel() { }

    public ComponentModel(string content, ComponentType componentType)
    {
        Content = content;
        ComponentType = componentType;
    }

    public ComponentModel(string content, ComponentType componentType, Point startPosition, Point endPosition)
    {
        Content = content;
        ComponentType = componentType;
        StartPosition = startPosition;
        EndPosition = endPosition;
    }

    public ComponentModel(string token, string content, ComponentType componentType)
    {
        Token = token;
        Content = content;
        ComponentType = componentType;
    }

    public ComponentModel(string token, string content, ComponentType componentType, Point startPosition, Point endPosition)
    {
        Token = token;
        Content = content;
        ComponentType = componentType;
        StartPosition = startPosition;
        EndPosition = endPosition;
    }
}