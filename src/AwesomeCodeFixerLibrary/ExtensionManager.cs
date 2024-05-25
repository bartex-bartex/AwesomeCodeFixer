using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;
using System.Diagnostics;
using System.Drawing;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace AwesomeCodeFixerLibrary
{
    public static class ExtensionManager
    {
        public static List<ErrorModel> LintCode(string content)
        {
            Stopwatch stopwatch = new Stopwatch();
            List<ErrorModel> output = new();

            ComponentModel rootComponent = Decompose(content, true);
            var codeBlockComponents = Converter.GetCodeBlockComponentTypes();

            // Linting part
            List<ComponentModel> current = new() { rootComponent };
            List<ComponentModel> next = new();

            while (current.Count > 0)
            {
                foreach (var component in current)
                {
                    string componentContent = component.Content;

                    // Extract code from code block
                    if (codeBlockComponents.Contains(component.ComponentType))
                    {
                        componentContent = ExtractCodeFromCodeBlock(componentContent);
                    }

                    // Lint component
                    stopwatch.Start();

                    string lintOutput = Linter.Lint(componentContent, component.ComponentType);

                    stopwatch.Stop();

                    component.Errors = ErrorManager.DeserializeIssues(lintOutput, component.ComponentType);
                    output.AddRange(component.Errors);

                    // Offset error location
                    OffsetErrorLocation(component);

                    next.AddRange(component.Children);
                }
                
                current.Clear();
                current.AddRange(next);
                next.Clear();
            }

            output.Sort();

            Console.WriteLine($"Linting time elapsed: {stopwatch.Elapsed}");

            return output;
        }

        private static void OffsetErrorLocation(ComponentModel component)
        {
            List<TokenDetailDto> tokenDetails = component.Children.Select(x => new TokenDetailDto(x.Token, x.StartPosition, x.EndPosition)).ToList();
            tokenDetails.ForEach(t => t.RelativeStartPosition = GetTokenRelativeStartPosition(component.Content, t.Token));
            tokenDetails = tokenDetails.Where(x => x.RelativeStartPosition.X >= 0).ToList();
            tokenDetails.Sort();

            var codeBlockTypes = Converter.GetCodeBlockComponentTypes();

            foreach (var error in component.Errors)
            {
                // Find the closest preceding token
                TokenDetailDto? mostPrecedingToken = GetMostPrecedingToken(error.Row, error.Column, tokenDetails);

                // meaning no token preceding the error
                if (mostPrecedingToken == null)
                {
                    error.Row += component.StartPosition.X - 1;

                    if (codeBlockTypes.Contains(component.ComponentType)) { error.Row += 1; }

                    continue;
                }

                // Relative token end position
                Point tokenRelativeEndPosition = new(mostPrecedingToken.RelativeStartPosition.X, mostPrecedingToken.RelativeStartPosition.Y + mostPrecedingToken.Token.Length);

                // Offset error location according to the most preceding token end position
                int a = error.Row - tokenRelativeEndPosition.X;
                int b = error.Column - tokenRelativeEndPosition.Y;

                // Applicate offset
                error.Row = a + mostPrecedingToken.EndPosition.X;
                if (codeBlockTypes.Contains(component.ComponentType)) { error.Row += 1; }

                if (a == 0) { error.Column = b + mostPrecedingToken.EndPosition.Y; }
            }
        }

        private static TokenDetailDto? GetMostPrecedingToken(int errorRow, int errorColumn, List<TokenDetailDto> tokenDetails)
        {
            TokenDetailDto? mostPrecedingToken = null;

            foreach (var token in tokenDetails)
            {
                if (token.RelativeStartPosition.X > errorRow 
                    || (token.RelativeStartPosition.X == errorRow && token.RelativeStartPosition.Y > errorColumn))
                {
                    break;
                }

                mostPrecedingToken = token;
            }

            return mostPrecedingToken;
        }

        public static string FormatCode(string content)
        {
            ComponentModel rootComponent = Decompose(content, false);
            var codeBlockComponents = Converter.GetCodeBlockComponentTypes();

            // Formatting part
            List<ComponentModel> current = new() { rootComponent };
            List<ComponentModel> next = new();

            while (current.Count > 0)
            {
                foreach (var component in current)
                {
                    string componentContent = component.Content;

                    // Extract code from code block
                    if (codeBlockComponents.Contains(component.ComponentType))
                    {
                        componentContent = ExtractCodeFromCodeBlock(componentContent);
                    }

                    // Format component
                    // TODO - output comes without polish characters!
                    string formatOutput = Formatter.Format(componentContent, component.ComponentType);

                    // Replace content with formatted content
                    if (codeBlockComponents.Contains(component.ComponentType))
                    {
                        component.Content = PutCodeIntoCodeBlock(formatOutput, component.ComponentType);
                    }
                    else
                    {
                        component.Content = formatOutput;
                    }

                    next.AddRange(component.Children);
                }
                
                current.Clear();
                current.AddRange(next);
                next.Clear();
            }

            content = Compose(rootComponent);

            return content;
        }

        private static string Compose(ComponentModel rootComponent)
        {
            string rootContent = rootComponent.Content;

            foreach (ComponentModel component in rootComponent.Children)
            {
                if (component.Children.Count > 0)
                {
                    string childContent = Compose(component);

                    // Above string contains full legit content
                    rootContent = rootContent.Replace(component.Token, childContent);
                }
                else
                {
                    rootContent = rootContent.Replace(component.Token, component.Content);
                }

            } 

            return rootContent;
        }

        private static ComponentModel Decompose(string content, bool appendComponentPosition)
        {
            ComponentModel rootComponent = new(content, ComponentType.Markdown, 
                    new Point(1, 1), GetComponentEndPositon(content, new Point(1, 1)));
            List<ComponentModel> next = new()
            { 
                rootComponent
            };

            while (next.Count > 0)
            {
                ComponentModel current = next[0];
                next.RemoveAt(0);

                // Skip non-nesting components
                if (!Converter.GetNestingComponents().Contains(current.ComponentType)) { continue;}

                // Add children and fill their data
                string parentOriginalContent = current.Content;
                foreach (ComponentType componentType in Converter.GetComponentTypes())
                {
                    string pattern = Converter.ConvertComponentTypeToPattern(componentType);

                    foreach (string match in Regex.Matches(current.Content, pattern).Select(x => x.Value))
                    {
                        // Do not allow to match the whole content <=> infinite loop
                        if (match.Length == current.Content.Length) { continue; }

                        // E.g. for latex we need to match 1 char before to exclude "\$" possibility, now it is reversed
                        string normalizedMatch = NormalizeMatch(componentType, match);

                        string token = Guid.NewGuid().ToString();

                        // Create child
                        ComponentModel child = new ComponentModel(token, normalizedMatch, componentType);
                        current.Content = current.Content.Replace(normalizedMatch, token);

                        if (appendComponentPosition)
                        {
                            child.StartPosition = GetComponentStartPosition(parentOriginalContent, normalizedMatch, current.StartPosition);
                            child.EndPosition = GetComponentEndPositon(normalizedMatch, child.StartPosition);
                        }

                        // Add child
                        current.Children.Add(child);
                        next.Add(child);
                    }
                }
            }

            return rootComponent;
        }

        private static string NormalizeMatch(ComponentType componentType, string match)
        {
            string matchCopy = match;

            // I am matching Latex and 1 char before to exclude "\$" possibility
            if (componentType == ComponentType.InlineLatex
                    || componentType == ComponentType.BlockLatex)
            {
                matchCopy = NormalizeLatex(match);
            }

            return matchCopy;
        }

        private static string NormalizeLatex(string latex)
        {
            if (latex[1] == '$' && latex[2] == '$') 
                return latex.Substring(1);

            else if (latex[1] == '$') 
                return latex.Substring(1);

            return latex;
        }

        private static Point GetComponentStartPosition(string parentOriginalContent, string match, Point parentPosition)
        {
            int pos = parentOriginalContent.IndexOf(match);
            List<string> contentBeforeComponent = parentOriginalContent.Substring(0, pos).Split(Environment.NewLine).ToList();

            int row = contentBeforeComponent.Count + parentPosition.X - 1;
            int column = contentBeforeComponent[^1].Length + 1;

            return new Point(row, column);
        }

        // TODO - optimize (do not split)
        private static Point GetComponentEndPositon(string content, Point startPosition)
        {
            List<string> contentLines = content.Split(Environment.NewLine).ToList();

            int row = startPosition.X + contentLines.Count - 1;
            int column = contentLines[^1].Length;

            return new Point(row, column);
        }

        private static Point GetTokenRelativeStartPosition(string component, string token)
        {
            int pos = component.IndexOf(token);

            // Token get encapsulated in another token
            if (pos == -1) { return new Point(-1, -1);}

            string[] contentBeforeToken = component.Substring(0, pos).Split(Environment.NewLine);

            return new Point(contentBeforeToken.Length, contentBeforeToken[^1].Length + 1);
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