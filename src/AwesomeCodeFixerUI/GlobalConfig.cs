using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace AwesomeCodeFixerUI;

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
}