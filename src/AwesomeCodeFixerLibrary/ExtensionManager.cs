using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;
using System.Diagnostics;
using System.Drawing;

namespace AwesomeCodeFixerLibrary
{
    public static class ExtensionManager
    {
        public static List<ErrorModel> LintCode(string content)
        {
            List<ErrorModel> output = new();

            var components = Decompose(ref content, true);

            string lintOutput = Linter.Lint(content, ComponentType.Markdown);
            output.AddRange(ErrorManager.DeserializeIssues(lintOutput, ComponentType.Markdown));

            var codeBlockComponents = Converter.GetCodeBlockComponentTypes();
            foreach (var component in components.Values)
            {
                string componentContent = component.Content;

                if (codeBlockComponents.Contains(component.ComponentType))
                {
                    componentContent = ExtractCodeFromCodeBlock(componentContent);
                }

                lintOutput = Linter.Lint(componentContent, component.ComponentType, component.Position);
                output.AddRange(ErrorManager.DeserializeIssues(lintOutput, component.ComponentType));
            }

            output.Sort();

            return output;
        }

        private static Point RecalculateErrorLocation()
        {
            return Point.Empty;
        }

        // public static string FormatCode(string content)
        // {
        //     var components = Decompose(ref content, false);

        //     content = Formatter.Format(content, ComponentType.Markdown);

        //     foreach (var component in components.Values)
        //     {
        //         if (component.ComponentType == ComponentType.CodeBlock)
        //         {
        //             string formattedComponent = Formatter.Format(ExtractCodeFromCodeBlock(component.Content), 
        //                             component.ComponentType, component.Language);
        //             component.Content = PutCodeIntoCodeBlock(formattedComponent, component.Language);
        //         }
        //         else
        //         {
        //             string formattedComponent = Formatter.Format(component.Content, component.ComponentType);
        //             component.Content = formattedComponent;
        //         }
        //     }

        //     content = Compose(content, components);

        //     return content;
        // }

        private static string Compose(string content, Dictionary<string, ComponentModel> componentsByTokens)
        {
            foreach (var pair in componentsByTokens.Reverse())
            {
                content = content.Replace(pair.Key, pair.Value.Content);
            }

            return content;
        }

        private static Dictionary<string, ComponentModel> Decompose(ref string content, bool appendComponentPosition)
        {
            int tokenCounter = 0;
            string contentCopy = content;
            Dictionary<string, ComponentModel> output = new();

            foreach (ComponentType componentType in Converter.GetComponentTypes())
            {
                string pattern = Converter.ConvertComponentTypeToPattern(componentType);

                foreach (string match in Regex.Matches(content, pattern).Select(x => x.Value))
                {
                    string token = $"token{tokenCounter}";
                    string matchCopy = match;

                    // I am matching Latex and 1 char before to exclude "\$" possibility
                    if (componentType == ComponentType.InlineLatex 
                            || componentType == ComponentType.BlockLatex)
                    {
                        matchCopy = NormalizeLatex(matchCopy);
                    }

                    output.Add(token, new ComponentModel(matchCopy, componentType));

                    if (appendComponentPosition)
                    {
                        output[token].Position = GetComponentPosition(contentCopy, output, componentType, matchCopy);
                    }

                    content = content.Replace(matchCopy, token);
                    tokenCounter++;
                }
            }

            return output;
        }

        private static string NormalizeLatex(string latex)
        {
            if (latex[1] == '$' && latex[2] == '$') 
                return latex.Substring(1);

            else if (latex[1] == '$') 
                return latex.Substring(1);

            return latex;
        }

        private static Point GetComponentPosition(string fullContent, Dictionary<string, ComponentModel> replacedTokens,
                                     ComponentType componentType, string match)
        {
            string? foundToken;

            // if match contains tokens, replace them
            if (Converter.GetNestingComponents().Contains(componentType))
            {
                while ((foundToken = replacedTokens.Keys.FirstOrDefault(t => match.Contains(t))) != null)
                {
                    match = Regex.Replace(match, foundToken, replacedTokens[foundToken].Content);
                }
            }

            // if match contains code block, offset it by one line
            int lineOffset = 0;
            if (Converter.GetCodeBlockComponentTypes().Contains(componentType)) { lineOffset = 1; }

            // TODO - remove | append position data to ComponentModel
            //output[token].Position = GetComponentPosition(contentCopy, matchCopy, lineOffset);
            
            // Get position of the component
            int pos = fullContent.IndexOf(match);
            List<string> contentBeforeComponent = fullContent.Substring(0, pos).Split(Environment.NewLine).ToList();

            int line = contentBeforeComponent.Count + lineOffset;
            int column = contentBeforeComponent[^1].Length + 1;

            return new Point(line, column);
        }

        private static string ExtractCodeFromCodeBlock(string codeBlock)
        {
            // TODO - compare performance
            // codeBlock = Regex.Replace(codeBlock, $@"```(?:\w+{Environment.NewLine}|{Environment.NewLine}```)", "");

            codeBlock = Regex.Replace(codeBlock, $@"```\w+{Environment.NewLine}", "");
            codeBlock = Regex.Replace(codeBlock, $@"{Environment.NewLine}```", "");

            return codeBlock;
        }

        private static string PutCodeIntoCodeBlock(string code, ComponentType language)
        {
            return $"```{Converter.ConvertComponentTypeToLanguage(language)}{Environment.NewLine}{code}{Environment.NewLine}```";
        }
    }
}
