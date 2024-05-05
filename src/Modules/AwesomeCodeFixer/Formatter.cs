using System.Diagnostics;

namespace AwesomeCodeFixer;

public static class Formatter
{
    /// <summary>
    /// Method formats content and returns formatted content as string.
    /// </summary>
    /// <param name="content">Component content</param>
    /// <param name="componentType">Type of component</param>
    /// <returns>String with formatted content</returns>
    public static string Format(string content, ComponentType componentType)
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
            case ComponentType.Sql:
                command = $"npx prettier --stdin-filepath foo.sql";
                break;
            case ComponentType.Cpp:
                command = $"clang-format.exe --assume-filename=foo.cpp";
                break;
            case ComponentType.C:
                command = $"clang-format.exe --assume-filename=foo.c";
                break;
            case ComponentType.Python:
                command = $"black -";
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
        output = Helper.TrimLines(output, 8, 2);

        return output;
    }
}