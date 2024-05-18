using System.Diagnostics;

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
        string output = "";

        string command = "";
        switch (componentType)
        {
            case ComponentType.Markdown:
                command = $"npx eslint --stdin --stdin-filename=foo.md";
                //command = @$"cat {filePath} | npx eslint --stdin --stdin-filename match_format.md"
                break;
            case ComponentType.InlineLatex:
            case ComponentType.BlockLatex:
                command = $"chktex";
                break;
            case ComponentType.Cpp:
                command = $"clang-format --dry-run --Werror --assume-filename=.cpp";
                break;
            case ComponentType.C:
                command = $"clang-format --dry-run --Werror --assume-filename=.c";
                break;
            case ComponentType.Python:
                command = $"flake8 -";
                break;
            case ComponentType.Sql:
                command = $"sqlfluff lint - --dialect ansi";
                break;
            default:
                break;
        }

        using (Process linter = new Process())
        {
            if (componentType == ComponentType.InlineLatex 
                    || componentType == ComponentType.BlockLatex)
            {
                // UseShellExecute = false => ok, when FileName is an .exe 
                linter.StartInfo.FileName = "ubuntu2204";

                // /c => Carries out the command specified by string and then terminates
                linter.StartInfo.RedirectStandardInput = true;
                linter.StartInfo.RedirectStandardOutput = true;
                // linter.StartInfo.RedirectStandardError = true; // For additional summarization

                linter.StartInfo.UseShellExecute = false;
                linter.StartInfo.CreateNoWindow = true;
                linter.Start();
                
                linter.StandardInput.NewLine = "\n";
                linter.StandardInput.WriteLine(command);
                linter.StandardInput.WriteLine(content);
                linter.StandardInput.WriteLine("^D"); // TODO - probably not needed
                linter.StandardInput.Close();

                // Waits until process terminates
                output = linter.StandardOutput.ReadToEnd();

                linter.WaitForExit();
            }
            else
            {
                // UseShellExecute = false => ok, when FileName is an .exe 
                linter.StartInfo.FileName = "cmd";

                // /c => Carries out the command specified by string and then terminates
                linter.StartInfo.RedirectStandardInput = true;
                linter.StartInfo.RedirectStandardOutput = true;
                // linter.StartInfo.RedirectStandardError = true; // For additional summarization

                linter.StartInfo.UseShellExecute = false;
                linter.StartInfo.CreateNoWindow = true;
                linter.Start();
                
                linter.StandardInput.WriteLine(command);
                linter.StandardInput.WriteLine(content);
                linter.StandardInput.WriteLine("^D"); // TODO - probably not needed
                linter.StandardInput.Close();

                // Waits until process terminates
                output = linter.StandardOutput.ReadToEnd();

                linter.WaitForExit();
            }
        }

        return output;
    }
}