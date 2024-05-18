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

        string command = "";
        switch (componentType)
        {
            case ComponentType.Markdown:
                command = $"npx prettier --stdin-filepath foo.md";
                break;
            case ComponentType.InlineLatex:
            case ComponentType.BlockLatex:
                command = $"npx prettier --stdin-filepath foo.tex";
                break;
            case ComponentType.CodeBlock:
                switch (language)
                {
                    case Language.cpp:
                        command = $"clang-format --assume-filename=foo.cpp";
                        break;
                    case Language.c:
                        command = $"clang-format --assume-filename=foo.c";
                        break;
                    case Language.python:
                        command = $"black -";
                        break;
                    case Language.sql:
                        command = $"npx prettier --stdin-filepath foo.sql";
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
                command = "echo 'No formatting for this component'";
                break;
            default:
                break;
        }

        using (Process formatter = new Process())
        {
            // UseShellExecute = false => ok, when FileName is an .exe 
            formatter.StartInfo.FileName = "cmd";

            // /c => Carries out the command specified by string and then terminates
            formatter.StartInfo.RedirectStandardInput = true;
            formatter.StartInfo.RedirectStandardOutput = true;
            formatter.StartInfo.RedirectStandardError = true; // For additional summarization

            formatter.StartInfo.UseShellExecute = false;
            formatter.StartInfo.CreateNoWindow = true;
            formatter.Start();
            
            formatter.StandardInput.NewLine = Environment.NewLine;
            formatter.StandardInput.WriteLine(command);
            formatter.StandardInput.WriteLine(content);
            formatter.StandardInput.Close();

            // Waits until process terminates
            output = formatter.StandardOutput.ReadToEnd();

            formatter.WaitForExit();
        }

        // TODO - Figure out why 8 and 2, should be 4 and 1.
        //output = Helper.TrimLines(output, 8, 2);

        return output;
    }
}