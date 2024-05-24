using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace AwesomeCodeFixerLibrary;

public static class Linter
{
    /// <summary>
    /// Method creates a file with content and returns string with linting warnings and errors. 
    /// </summary>
    /// <param name="content">Component content</param>
    /// <param name="componentType">Type of component</param>
    /// <returns>String with linting warnings and errors</returns>
    public static string Lint(string content, ComponentType componentType)
    {
        string filename = "";
        string arguments = "";
        string codeFilename = "";
        switch (componentType)
        {
            case ComponentType.Markdown:
                filename = @"C:\Program Files\nodejs\npx.cmd";
                arguments = $"eslint --stdin --stdin-filename=foo.md";
                break;
            case ComponentType.InlineLatex:
            case ComponentType.BlockLatex:
                filename = $@"chktex -f '%l %c %m {Environment.NewLine}' -q";
                break;
            case ComponentType.CppCodeBlock:
                filename = @"C:\Users\Bartek\AppData\Local\Programs\Python\Python312\Scripts\clang-tidy.exe";
                arguments = $"--quiet temp.cpp --";
                codeFilename = "temp.cpp";
                break;
            case ComponentType.CCodeBlock:
                filename = @"C:\Users\Bartek\AppData\Local\Programs\Python\Python312\Scripts\clang-tidy.exe";
                arguments = $"--quiet temp.c --";
                codeFilename = "temp.c";
                break;
            case ComponentType.PythonCodeBlock:
                filename = @"C:\Users\Bartek\AppData\Local\Programs\Python\Python312\Scripts\flake8.exe";
                arguments = $"-";
                break;
            case ComponentType.SqlCodeBlock:
                filename = @"C:\Users\Bartek\AppData\Local\Programs\Python\Python312\Scripts\sqlfluff.exe";
                arguments = $"lint - --format github-annotation --dialect ansi";
                break;
            case ComponentType.UnspecifiedCodeBlock:
                return "";
            // TODO - Try HTML linter
            case ComponentType.YouTube:
            case ComponentType.Info:
            case ComponentType.Note:
            case ComponentType.Warning:
            case ComponentType.DeepDive:
                return "";
            default:
                break;
        }

        if (!string.IsNullOrEmpty(codeFilename))
        {
            File.WriteAllText(codeFilename, content);
        }

        StringBuilder outputBuilder = new StringBuilder();
        StringBuilder errorBuilder = new StringBuilder();

        using (Process linter = new Process())
        {

            if (componentType == ComponentType.InlineLatex 
                    || componentType == ComponentType.BlockLatex)
            {
                linter.StartInfo.FileName = "ubuntu2204";

                linter.StartInfo.RedirectStandardInput = true;
                linter.StartInfo.RedirectStandardOutput = true;
                linter.StartInfo.RedirectStandardError = true; // For additional summarization

                linter.StartInfo.UseShellExecute = false;
                linter.StartInfo.CreateNoWindow = true;

                linter.OutputDataReceived += (sender, e) => outputBuilder.AppendLine(e.Data);
                linter.ErrorDataReceived += (sender, e) => errorBuilder.AppendLine(e.Data);

                linter.Start();
                
                linter.StandardInput.NewLine = "\n";
                linter.StandardInput.WriteLine($"{filename}");
                linter.StandardInput.WriteLine(content);
                linter.StandardInput.Close();

                linter.BeginOutputReadLine();
                linter.BeginErrorReadLine();

                linter.WaitForExit();
            }
            else if (componentType == ComponentType.CppCodeBlock || componentType == ComponentType.CCodeBlock)
            {
                linter.StartInfo.FileName = filename;
                linter.StartInfo.Arguments = arguments;

                linter.StartInfo.RedirectStandardOutput = true;
                linter.StartInfo.RedirectStandardError = true; // For additional summarization

                linter.StartInfo.UseShellExecute = false;
                linter.StartInfo.CreateNoWindow = true;

                linter.OutputDataReceived += (sender, e) => outputBuilder.AppendLine(e.Data);
                linter.ErrorDataReceived += (sender, e) => errorBuilder.AppendLine(e.Data);

                linter.Start();
                
                //linter.StandardInput.Close();

                // Freezes when output is too big and fills whole buffer
                //output = linter.StandardOutput.ReadToEnd();

                linter.BeginOutputReadLine();
                linter.BeginErrorReadLine();

                linter.WaitForExit();
            }
            else
            {
                linter.StartInfo.FileName = filename;
                linter.StartInfo.Arguments = arguments;

                linter.StartInfo.RedirectStandardInput = true;
                linter.StartInfo.RedirectStandardOutput = true;
                linter.StartInfo.RedirectStandardError = true; // For additional summarization

                linter.StartInfo.UseShellExecute = false;
                linter.StartInfo.CreateNoWindow = true;

                linter.OutputDataReceived += (sender, e) => outputBuilder.AppendLine(e.Data);
                linter.ErrorDataReceived += (sender, e) => errorBuilder.AppendLine(e.Data);

                linter.Start();
                
                linter.StandardInput.NewLine = Environment.NewLine;
                //linter.StandardInput.WriteLine(command);

                linter.StandardInput.WriteLine(content);
                linter.StandardInput.Close();

                // Freezes when output is too big and fills whole buffer
                //output = linter.StandardOutput.ReadToEnd();

                linter.BeginOutputReadLine();
                linter.BeginErrorReadLine();

                linter.WaitForExit();
            }
        }

        if (!string.IsNullOrEmpty(codeFilename))
        {
            File.Delete(codeFilename);
        }

        return outputBuilder.ToString();
    }
}