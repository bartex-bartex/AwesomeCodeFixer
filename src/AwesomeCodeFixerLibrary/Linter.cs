using System.Diagnostics;
using System.Drawing;

namespace AwesomeCodeFixerLibrary;

public static class Linter
{
    /// <summary>
    /// Method creates a file with content and returns string with linting warnings and errors. 
    /// </summary>
    /// <param name="content">Component content</param>
    /// <param name="componentType">Type of component</param>
    /// <returns>String with linting warnings and errors</returns>
    public static string Lint(string content, ComponentType componentType, Point componentPosition = default(Point))
    {
        string output = "";

        string filename = "";
        string arguments = "";
        switch (componentType)
        {
            case ComponentType.Markdown:
                filename = @"C:\Program Files\nodejs\npx.cmd";
                arguments = $"eslint --stdin --stdin-filename=foo.md";
                break;
            case ComponentType.InlineLatex:
            case ComponentType.BlockLatex:
                filename = @$"chktex -f '%l %c %m \n' -q";
                break;
            case ComponentType.CppCodeBlock:
                filename = @"C:\Users\Bartek\AppData\Local\Programs\Python\Python312\Scripts\clang-format.exe";
                arguments = $"--dry-run --Werror --assume-filename=foo.cpp";
                break;
            case ComponentType.CCodeBlock:
                filename = @"C:\Users\Bartek\AppData\Local\Programs\Python\Python312\Scripts\clang-format.exe";
                arguments = $"--dry-run --Werror --assume-filename=foo.c";
                break;
            case ComponentType.PythonCodeBlock:
                filename = @"C:\Users\Bartek\AppData\Local\Programs\Python\Python312\Scripts\flake8.exe";
                arguments = $"-";
                break;
            case ComponentType.SqlCodeBlock:
                filename = @"C:\Users\Bartek\AppData\Local\Programs\Python\Python312\Scripts\sqlfluff.exe";
                arguments = $"lint - --dialect ansi";
                break;
            // TODO - Try HTML linter
            case ComponentType.YouTube:
            case ComponentType.Info:
            case ComponentType.Note:
            case ComponentType.Warning:
            case ComponentType.DeepDive:
                Debug.WriteLine("echo 'No Linting for this component'");
                return "";
                break;
            default:
                break;
        }

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
                linter.Start();
                
                linter.StandardInput.NewLine = "\n";
                linter.StandardInput.WriteLine($"{filename}");
                linter.StandardInput.WriteLine(content);
                linter.StandardInput.Close();

                // Waits until process terminates
                output = linter.StandardOutput.ReadToEnd();

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
                linter.Start();
                
                linter.StandardInput.NewLine = Environment.NewLine;
                //linter.StandardInput.WriteLine(command);
                linter.StandardInput.WriteLine(content);
                linter.StandardInput.Close();

                // Waits until process terminates
                output = linter.StandardOutput.ReadToEnd();

                linter.WaitForExit();
            }
        }

        Debug.WriteLine(componentPosition.ToString());

        return output;
    }
}