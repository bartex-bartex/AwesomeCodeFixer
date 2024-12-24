using System.Diagnostics;
using System.Text;

namespace AwesomeCodeFixerLibrary;

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
        string filename = "";
        string arguments = "";
        switch (componentType)
        {
            case ComponentType.Markdown:
                filename = @"npx";
                arguments = $"prettier --stdin-filepath foo.md";
                break;
            case ComponentType.InlineLatex:
            case ComponentType.BlockLatex:
                filename = @"npx";
                arguments = $"prettier --stdin-filepath foo.tex";
                break;
            case ComponentType.CppCodeBlock:
                filename = @"clang-format";
                arguments = $"--assume-filename=foo.cpp";
                break;
            case ComponentType.CCodeBlock:
                filename = @"clang-format";
                arguments = $"--assume-filename=foo.c";
                break;
            case ComponentType.PythonCodeBlock:
                filename = @"black";
                arguments = $"-";
                break;
            case ComponentType.SqlCodeBlock:
                filename = @"npx";
                arguments = $"prettier --stdin-filepath foo.sql";
                break;
            case ComponentType.UnspecifiedCodeBlock:
                return content;
            case ComponentType.YouTube:
            case ComponentType.Info:
            case ComponentType.Note:
            case ComponentType.Warning:
            case ComponentType.DeepDive:
                return content;
            default:
                break;
        }

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        StringBuilder outputBuilder = new StringBuilder();

        using (Process formatter = new Process())
        {
            formatter.StartInfo.FileName = filename;
            formatter.StartInfo.Arguments = arguments;

            formatter.StartInfo.RedirectStandardInput = true;
            formatter.StartInfo.RedirectStandardOutput = true;
            formatter.StartInfo.RedirectStandardError = true;

            formatter.StartInfo.UseShellExecute = false;
            formatter.StartInfo.CreateNoWindow = true;

            formatter.OutputDataReceived += (sender, e) => outputBuilder.AppendLine(e.Data);

            formatter.Start();
            
            formatter.StandardInput.NewLine = Environment.NewLine;
            formatter.StandardInput.WriteLine(content);
            formatter.StandardInput.Close();

            formatter.BeginOutputReadLine();

            formatter.WaitForExit();
        }

        stopwatch.Stop();
        Console.WriteLine($"{componentType} | Time elapsed: {stopwatch.Elapsed}");

        return outputBuilder.ToString();
    }
}