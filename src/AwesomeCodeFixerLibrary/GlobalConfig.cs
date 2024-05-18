using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace AwesomeCodeFixerLibrary;

public static class GlobalConfig
{
    public static readonly ReadOnlyDictionary<ComponentType, string> ComponentToExtension = new(
        new Dictionary<ComponentType, string>
        {
            { ComponentType.Markdown, ".md" },
            { ComponentType.InlineLatex, ".tex" },
            { ComponentType.BlockLatex, ".tex" },
            { ComponentType.Cpp, ".cpp" },
            { ComponentType.C, ".c" },
            { ComponentType.Python, ".py" },
            { ComponentType.Sql, ".sql" }
        }
    );

    public static readonly ReadOnlyDictionary<string, Language> LanguageByString = new(
        new Dictionary<string, Language>
        {
            {"cpp", Language.Cpp},
            {"c", Language.C},
            {"python", Language.Python},
            {"sql", Language.Sql}
        }
    );

}