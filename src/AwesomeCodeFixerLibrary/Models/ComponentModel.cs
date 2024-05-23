using System.Drawing;

namespace AwesomeCodeFixerLibrary; 

public class ComponentModel
{
    public string Token { get; set; }
    public string Content { get; set; }
    public ComponentType ComponentType { get; set; }
    public Point Position { get; set; }
    public List<ErrorModel> Errors { get; set; }
    public List<ComponentModel> Children { get; set; }

    public ComponentModel(string content, ComponentType componentType)
    {
        Content = content;
        ComponentType = componentType;
    }
}