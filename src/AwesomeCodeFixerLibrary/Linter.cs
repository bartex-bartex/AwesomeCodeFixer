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
                filename = @"/home/bartex/.nvm/versions/node/v20.13.1/bin/npx";
                arguments = $"eslint --stdin --stdin-filename=foo.md";
                break;
            case ComponentType.InlineLatex:
            case ComponentType.BlockLatex:
                filename = @"/usr/bin/chktex";
                arguments = $@"-f%l:%c:%m|n| -q";
                break;
            case ComponentType.CppCodeBlock:
                filename = @"/home/bartex/GithubProjects/AwesomeCodeFixer/.venv/bin/clang-tidy";
                arguments = $"--quiet temp.cpp --";
                codeFilename = "temp.cpp";
                break;
            case ComponentType.CCodeBlock:
                filename = @"/home/bartex/GithubProjects/AwesomeCodeFixer/.venv/bin/clang-tidy";
                arguments = $"--quiet temp.c --";
                codeFilename = "temp.c";
                break;
            case ComponentType.PythonCodeBlock:
                filename = @"/home/bartex/GithubProjects/AwesomeCodeFixer/.venv/bin/flake8";
                arguments = $"-";
                break;
            case ComponentType.SqlCodeBlock:
                filename = @"/home/bartex/GithubProjects/AwesomeCodeFixer/.venv/bin/sqlfluff";
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

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        // Create temp file
        if (!string.IsNullOrEmpty(codeFilename))
        {
            File.WriteAllText(codeFilename, content);
        }

        StringBuilder outputBuilder = new StringBuilder();

        // Without the shell you do not have access to PATH
        using (Process linter = new Process())
        {
            linter.StartInfo.FileName = filename;
            linter.StartInfo.Arguments = arguments;

            // Process uses its own path, npx uses node, which it can't find, so I put its folder in the process PATH
            // Strange because printing shows normal Shell PATH
            linter.StartInfo.RedirectStandardInput = true;
            linter.StartInfo.RedirectStandardOutput = true;
            linter.StartInfo.RedirectStandardError = true;

            // true means "behave as user double-click the file"
            linter.StartInfo.UseShellExecute = false;
            linter.StartInfo.CreateNoWindow = true;

            linter.OutputDataReceived += (sender, e) => outputBuilder.AppendLine(e.Data);

            linter.Start();

            // Means read from stdin
            if (string.IsNullOrEmpty(codeFilename))
            {                
                linter.StandardInput.NewLine = Environment.NewLine;
                linter.StandardInput.WriteLine(content);
                linter.StandardInput.Close();
            }

            linter.BeginOutputReadLine();

            linter.WaitForExit();
        }

        // Remove temp file
        if (!string.IsNullOrEmpty(codeFilename))
        {
            File.Delete(codeFilename);
        }

        stopwatch.Stop();
        Console.WriteLine($"{componentType} | Time elapsed: {stopwatch.Elapsed}");

        return outputBuilder.ToString();
    }
}