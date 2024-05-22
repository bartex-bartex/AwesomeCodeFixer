using System.Drawing;

namespace AwesomeCodeFixerLibrary; 

public class ComponentModel
{
    public string Content { get; set; }
    public ComponentType ComponentType { get; set; }
    public Language Language { get; set; } = Language.unspecified;

    /// <summary>
    /// X - line, Y - column
    /// </summary>
    public Point Position { get; set; }

    public ComponentModel(string content, ComponentType componentType)
    {
        Content = content;
        ComponentType = componentType;
    }

        public ComponentModel(string content, ComponentType componentType, Language language)
    {
        Content = content;
        ComponentType = componentType;
        Language = language;
    }
}