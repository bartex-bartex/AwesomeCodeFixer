using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;
using System.Diagnostics;

namespace AwesomeCodeFixerLibrary
{
    public static class ExtensionManager
    {
        public static string LintCode(string content)
        {
            StringBuilder sb = new StringBuilder();

            var components = Decompose(ref content);

            foreach (var component in components.Values)
            {
                if (component.ComponentType == ComponentType.CodeBlock)
                {
                    string lintOutput = Linter.Lint(ExtractCodeFromCodeBlock(component.Content), component.ComponentType, component.Language);
                    sb.Append(lintOutput);
                }
                else
                {
                    string lintOutput = Linter.Lint(component.Content, component.ComponentType, component.Language);
                    sb.Append(lintOutput);
                }
            }

            return sb.ToString();
        }

        public static string FormatCode(string content)
        {
            var components = Decompose(ref content);

            content = Formatter.Format(content, ComponentType.Markdown);

            foreach (var component in components.Values)
            {
                if (component.ComponentType == ComponentType.CodeBlock)
                {
                    string formattedComponent = Formatter.Format(ExtractCodeFromCodeBlock(component.Content), component.ComponentType, component.Language);
                    component.Content = PutCodeIntoCodeBlock(formattedComponent, component.Language);
                }
                else
                {
                    string formattedComponent = Formatter.Format(component.Content, component.ComponentType);
                    component.Content = formattedComponent;
                }
            }

            content = Compose(content, components);

            return content;
        }

        private static string Compose(string content, Dictionary<string, ComponentModel> componentsByTokens)
        {
            foreach (var entry in componentsByTokens.Reverse())
            {
                content = content.Replace(entry.Key, entry.Value.Content);
            }

            return content;
        }

        private static Dictionary<string, ComponentModel> Decompose(ref string content)
        {
            int tokenCounter = 0;
            Dictionary<string, ComponentModel> componentsByTokens = new();

            Dictionary<ComponentType, string> componentsByPatterns = new()
            {
                { ComponentType.Header, @"---[\s\S]*?---" },
                { ComponentType.BlockLatex, @"\$\$[^\$]+\$\$" },
                { ComponentType.InlineLatex, @"\$[^\$]+\$" },
                { ComponentType.CodeBlock, @"```\w+[\s\S]*?```" },
                { ComponentType.YouTube, @"<YouTube[\s\S]*?/>" },
                // TODO - add support for NESTING THE SAME COMPONENTS
                { ComponentType.Info, @"<Info[\s\S]*?>[\s\S]*?</Info>" },
                { ComponentType.Note, @"<Note[\s\S]*?>[\s\S]*?</Note>" },
                { ComponentType.Warning, @"<Warning[\s\S]*?>[\s\S]*?</Warning>" },
                { ComponentType.DeepDive, @"<DeepDive[\s\S]*?>[\s\S]*?</DeepDive>" }
            };

            List<ComponentType> nestingComponents = new()
            {
                ComponentType.Info,
                ComponentType.Note,
                ComponentType.Warning,
                ComponentType.DeepDive
            };

            foreach (var entry in componentsByPatterns)
            {
                // TODO - fix SonarLint warning + PERFORMANCE check
                foreach (Match match in Regex.Matches(content, entry.Value))
                {
                    string key = $"token{tokenCounter}";

                    if (entry.Key == ComponentType.CodeBlock)
                    {
                        componentsByTokens.Add(key, 
                            new ComponentModel(match.Value, entry.Key, GetCodeBlockLanguage(match.Value)));
                    }
                    else
                    {
                        componentsByTokens.Add(key, new ComponentModel(match.Value, entry.Key));
                    }
                    
                    content = content.Replace(match.Value, key);
                    tokenCounter++;
                }
            }

            return componentsByTokens;
        }

        private static Language GetCodeBlockLanguage(string codeBlock)
        {
            string codeBlockLanguage = Regex.Match(codeBlock, @"\w+").ToString();
            Language lang;

            if (Enum.TryParse(typeof(Language), codeBlockLanguage.ToLower(), out object? parsedValue))
            {
                lang = (Language)parsedValue;
            }
            else
            {
                lang = Language.unspecified;
            }

            return lang;
        }

        private static string ExtractCodeFromCodeBlock(string codeBlock)
        {
            // TODO - compare performance
            // codeBlock = Regex.Replace(codeBlock, $@"```(?:\w+{Environment.NewLine}|{Environment.NewLine}```)", "");

            codeBlock = Regex.Replace(codeBlock, $@"```\w+{Environment.NewLine}", "");
            codeBlock = Regex.Replace(codeBlock, $@"{Environment.NewLine}```", "");

            return codeBlock;
        }

        private static string PutCodeIntoCodeBlock(string code, Language language)
        {
            return $"```{language.ToString()}{Environment.NewLine}{code}{Environment.NewLine}```";
        }
    }
}
