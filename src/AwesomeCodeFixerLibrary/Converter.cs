namespace AwesomeCodeFixerLibrary;

internal static class Converter
{
    private static Dictionary<string, ComponentType> stringToComponentType = new() 
    {
        { "cpp", ComponentType.CppCodeBlock },
        { "c", ComponentType.CCodeBlock },
        { "python", ComponentType.PythonCodeBlock },
        { "sql", ComponentType.SqlCodeBlock },
        { "" , ComponentType.UnspecifiedCodeBlock}
    };

    // TODO - check for single space between ``` and language name
    private static Dictionary<ComponentType, string> componentTypeToPattern = new()
    {
        // Nesting
        { ComponentType.Info, @"<Info[\s\S]*?>[\s\S]*?</Info>" },
        { ComponentType.Note, @"<Note[\s\S]*?>[\s\S]*?</Note>" },
        { ComponentType.Warning, @"<Warning[\s\S]*?>[\s\S]*?</Warning>" },
        { ComponentType.DeepDive, @"<DeepDive[\s\S]*?>[\s\S]*?</DeepDive>" },
        // Normal
        { ComponentType.BlockLatex, @"(^|[^\\])\$\$[^\$]+[^\\]\$\$" },
        { ComponentType.InlineLatex, @"(^|[^\\])\$[^\$]+[^\\]\$" },
        { ComponentType.CppCodeBlock, @"```[Cc]pp[\s\S]*?\r?\n```" },
        { ComponentType.CCodeBlock, @"```[Cc][\s\S]*?\r?\n```" },
        { ComponentType.PythonCodeBlock, @"```[Pp]ython[\s\S]*?\r?\n```" },
        { ComponentType.SqlCodeBlock, @"```[Ss]ql[\s\S]*?\r?\n```" },
        { ComponentType.UnspecifiedCodeBlock, @"```\w+[\s\S]*?\r?\n```" },
        { ComponentType.YouTube, @"<YouTube[\s\S]*?/>" }
    };

    private static List<ComponentType> nestingComponents = new()
    {
        ComponentType.Markdown,
        ComponentType.Info,
        ComponentType.Note,
        ComponentType.Warning,
        ComponentType.DeepDive
    };

    public static string ConvertComponentTypeToLanguage(ComponentType componentType)
    {
        return stringToComponentType.FirstOrDefault(x => x.Value == componentType).Key;
    }

    public static ComponentType ConvertLanguageToComponentType(string componentType)
    {
        return stringToComponentType[componentType];
    }

    public static string ConvertComponentTypeToPattern(ComponentType componentType)
    {
        return componentTypeToPattern[componentType];
    }

    public static ComponentType ConvertPatternToComponentType(string pattern)
    {
        return componentTypeToPattern.FirstOrDefault(x => x.Value == pattern).Key;
    }

    public static List<string> GetPatterns()
    {
        return componentTypeToPattern.Values.ToList();
    }

    public static List<ComponentType> GetComponentTypes()
    {
        return componentTypeToPattern.Keys.ToList();
    }

    public static List<ComponentType> GetCodeBlockComponentTypes()
    {
        return componentTypeToPattern.Keys.Where(x => x.ToString().Contains("CodeBlock")).ToList();
    }

    public static List<ComponentType> GetNestingComponents()
    {
        return nestingComponents;
    }

    public static List<ComponentType> GetNonNestingComponents()
    {
        return componentTypeToPattern.Keys.Except(nestingComponents).ToList();
    }
}