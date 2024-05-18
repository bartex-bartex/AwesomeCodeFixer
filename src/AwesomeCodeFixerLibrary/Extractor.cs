using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AwesomeCodeFixerLibrary
{
    public static class Extractor
    {

        public static Dictionary<string, string> ExtractComponents(ref string content)
        {
            Dictionary<string, string> componentsByTokens = new Dictionary<string, string>();
            int tokenCounter = 0;

            using (StringReader reader = new StringReader(content))
            {
                string? line = string.Empty;

                do
                {
                    line = reader.ReadLine();

                    if (line != null)
                    {
                        // code block
                        if (Regex.IsMatch(line, @"^```\s*\w"))
                        {
                            // read the whole code block
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine(line);
                            do
                            {
                                line = reader.ReadLine();
                                if (line != null)
                                {
                                    sb.AppendLine(line);
                                }
                            } while (line != null && !Regex.IsMatch(line, @"^```"));
                            string codeBlock = sb.ToString();

                            // extract language
                            Language lang = GetCodeBlockLanguage(codeBlock);

                            // replace code block with token and add it to dictionary
                            componentsByTokens.Add($"token{tokenCounter}", codeBlock);
                            content = content.Replace(codeBlock, $"token{tokenCounter}");
                            tokenCounter++;
                        }
                        // Custom Components - excluding Youtube
                        else if (Regex.IsMatch(line, @"^<\s*(Info|Note|Warning|DeepDive)"))
                        {
                            // Info, Note, Warning, DeepDive

                            // read the whole component
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine(line);
                            do
                            {
                                line = reader.ReadLine();
                                if (line != null)
                                {
                                    sb.AppendLine(line);
                                }
                            } while (line != null && !Regex.IsMatch(line, @"^</"));
                            string component = sb.ToString();

                            // replace code block with token and add it to dictionary
                            componentsByTokens.Add($"token{tokenCounter}", component);
                            content = content.Replace(component, $"token{tokenCounter}");
                            tokenCounter++;
                        }
                        // Youtube (auto-closed)
                        else if (Regex.IsMatch(line, @"<\s*Youtube"))
                        {
                            // read the whole component
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine(line);
                            while (line != null && !Regex.IsMatch(line, @"\\>"))
                            {
                                sb.AppendLine(line);
                                line = reader.ReadLine();
                            }
                            string youtube = sb.ToString();

                            // replace component with token and add it to dictionary
                            componentsByTokens.Add($"token{tokenCounter}", youtube);
                            content = content.Replace(youtube, $"token{tokenCounter}");
                            tokenCounter++;
                        }
                        // Latex inline / block
                        else if (Regex.IsMatch(line, @"(\$[^\$]+\$|\$\$[^\$]+\$\$)"))
                        {
                            // Extract latex part
                            string latex = Regex.Match(line, @"(\$[^\$]+\$|\$\$[^\$]+\$\$)").Value;

                            // read the whole latex
                            componentsByTokens.Add($"token{tokenCounter}", latex);
                            content = content.Replace(latex, $"token{tokenCounter}");
                            tokenCounter++;
                        }
                    }

                } while (line != null);
            }

            return componentsByTokens;
        }

        private static Language GetCodeBlockLanguage(string codeBlock)
        {
            string line = codeBlock.Split(Environment.NewLine, 1)[0];
            string codeBlockLanguage = Regex.Match(line, @"\w").ToString();
            Language lang;

            if (Enum.TryParse(typeof(Language), codeBlockLanguage, out object? parsedValue))
            {
                lang = (Language)parsedValue;
            }
            else
            {
                lang = Language.Unspecified;
            }

            return lang;
        }
    }
}
