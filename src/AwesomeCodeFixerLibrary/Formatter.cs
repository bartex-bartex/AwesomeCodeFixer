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
                filename = @"C:\Program Files\nodejs\npx.cmd";
                arguments = $"prettier --stdin-filepath foo.md";
                break;
            case ComponentType.InlineLatex:
            case ComponentType.BlockLatex:
                filename = @"C:\Program Files\nodejs\npx.cmd";
                arguments = $"prettier --stdin-filepath foo.tex";
                break;
            case ComponentType.CppCodeBlock:
                filename = @"C:\Users\Bartek\AppData\Local\Programs\Python\Python312\Scripts\clang-format.exe";
                arguments = $"--assume-filename=foo.cpp";
                break;
            case ComponentType.CCodeBlock:
                filename = @"C:\Users\Bartek\AppData\Local\Programs\Python\Python312\Scripts\clang-format.exe";
                arguments = $"--assume-filename=foo.c";
                break;
            case ComponentType.PythonCodeBlock:
                filename = @"C:\Users\Bartek\AppData\Local\Programs\Python\Python312\Scripts\black.exe";
                arguments = $"-";
                break;
            case ComponentType.SqlCodeBlock:
                filename = @"C:\Program Files\nodejs\npx.cmd";
                arguments = $"prettier --stdin-filepath foo.sql";
                break;
            case ComponentType.UnspecifiedCodeBlock:
                return content;
            // TODO - try HTML formatter
            case ComponentType.YouTube:
            case ComponentType.Info:
            case ComponentType.Note:
            case ComponentType.Warning:
            case ComponentType.DeepDive:
                return content;
            default:
                break;
        }

        StringBuilder outputBuilder = new StringBuilder();
        StringBuilder errorBuilder = new StringBuilder();

        using (Process formatter = new Process())
        {
            formatter.StartInfo.FileName = filename;
            formatter.StartInfo.Arguments = arguments;

            formatter.StartInfo.RedirectStandardInput = true;
            formatter.StartInfo.RedirectStandardOutput = true;
            formatter.StartInfo.RedirectStandardError = true; // For additional summarization

            formatter.StartInfo.UseShellExecute = false; // If false => you must specify a full path to .exe
            formatter.StartInfo.CreateNoWindow = true;

            formatter.OutputDataReceived += (sender, e) => outputBuilder.AppendLine(e.Data);
            formatter.ErrorDataReceived += (sender, e) => errorBuilder.AppendLine(e.Data);

            formatter.Start();
            
            formatter.StandardInput.NewLine = Environment.NewLine;
            //formatter.StandardInput.WriteLine(command);
            formatter.StandardInput.WriteLine(content);
            formatter.StandardInput.Close();

            // Waits until process terminates
            formatter.BeginOutputReadLine();
            formatter.BeginErrorReadLine();

            formatter.WaitForExit();
        }

        return outputBuilder.ToString();
    }
}