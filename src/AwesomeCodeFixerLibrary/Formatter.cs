using System.Diagnostics;

namespace AwesomeCodeFixerLibrary;

public static class Formatter
{
    /// <summary>
    /// Method formats content and returns formatted content as string.
    /// </summary>
    /// <param name="content">Component content</param>
    /// <param name="componentType">Type of component</param>
    /// <returns>String with formatted content</returns>
    public static string Format(string content, ComponentType componentType, Language? language = null)
    {
        string output = "";

        string filename = "";
        string arguments = "";
        switch (componentType)
        {
            case ComponentType.Markdown:
                filename = @"C:\Program Files\nodejs\npx.cmd";
                arguments = $"prettier --stdin-filepath foo.md";
                break;
            case ComponentType.InlineLatex:
            case ComponentType.BlockLatex:
                filename = @"C:\Program Files\nodejs\npx.cmd";
                arguments = $"prettier --stdin-filepath foo.tex";
                break;
            case ComponentType.CodeBlock:
                switch (language)
                {
                    case Language.cpp:
                        filename = @"C:\Users\Bartek\AppData\Local\Programs\Python\Python312\Scripts\clang-format.exe";
                        arguments = $"--assume-filename=foo.cpp";
                        break;
                    case Language.c:
                        filename = @"C:\Users\Bartek\AppData\Local\Programs\Python\Python312\Scripts\clang-format.exe";
                        arguments = $"--assume-filename=foo.c";
                        break;
                    case Language.python:
                        filename = @"C:\Users\Bartek\AppData\Local\Programs\Python\Python312\Scripts\black.exe";
                        arguments = $"-";
                        break;
                    case Language.sql:
                        filename = @"C:\Program Files\nodejs\npx.cmd";
                        arguments = $"prettier --stdin-filepath foo.sql";
                        break;
                    default:
                        break;
                }
                break;
            // TODO - try HTML formatter
            case ComponentType.YouTube:
            case ComponentType.Info:
            case ComponentType.Note:
            case ComponentType.Warning:
            case ComponentType.DeepDive:
                Debug.WriteLine("echo 'No formatting for this component'");
                return content;
                break;
            default:
                break;
        }

        using (Process formatter = new Process())
        {
            formatter.StartInfo.FileName = filename;
            formatter.StartInfo.Arguments = arguments;

            formatter.StartInfo.RedirectStandardInput = true;
            formatter.StartInfo.RedirectStandardOutput = true;
            formatter.StartInfo.RedirectStandardError = true; // For additional summarization

            formatter.StartInfo.UseShellExecute = false; // If false => you must specify a full path to .exe
            formatter.StartInfo.CreateNoWindow = true;
            formatter.Start();
            
            formatter.StandardInput.NewLine = Environment.NewLine;
            //formatter.StandardInput.WriteLine(command);
            formatter.StandardInput.WriteLine(content);
            formatter.StandardInput.Close();

            // Waits until process terminates
            output = formatter.StandardOutput.ReadToEnd();

            formatter.WaitForExit();
        }

        return output;
    }
}