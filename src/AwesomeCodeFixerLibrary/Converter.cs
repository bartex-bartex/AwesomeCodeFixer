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

    private static Dictionary<ComponentType, string> componentTypeToPattern = new()
    {
        //{ ComponentType.Header, @"---[\s\S]*?---" },
        { ComponentType.BlockLatex, @"\$\$[^\$]+\$\$" },
        { ComponentType.InlineLatex, @"\$[^\$]+\$" },
        { ComponentType.CppCodeBlock, @"```cpp[\s\S]*?```" },
        { ComponentType.CCodeBlock, @"```c[\s\S]*?```" },
        { ComponentType.PythonCodeBlock, @"```python[\s\S]*?```" },
        { ComponentType.SqlCodeBlock, @"```sql[\s\S]*?```" },
        { ComponentType.UnspecifiedCodeBlock, @"```\w+[\s\S]*?```" },
        { ComponentType.YouTube, @"<YouTube[\s\S]*?/>" },
        // TODO - add support for NESTING THE SAME COMPONENTS
        { ComponentType.Info, @"<Info[\s\S]*?>[\s\S]*?</Info>" },
        { ComponentType.Note, @"<Note[\s\S]*?>[\s\S]*?</Note>" },
        { ComponentType.Warning, @"<Warning[\s\S]*?>[\s\S]*?</Warning>" },
        { ComponentType.DeepDive, @"<DeepDive[\s\S]*?>[\s\S]*?</DeepDive>" }
    };

    private static List<ComponentType> nestingComponents = new()
    {
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
}