using System.Diagnostics;

namespace AwesomeCodeFixer;

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

        string filePath = $"temp{GlobalConfig.ComponentToExtension[componentType]}";
        File.WriteAllText(filePath, content);

        string command = "";
        switch (componentType)
        {
            case ComponentType.Markdown:
                command = $"npx eslint \"{filePath}\"";
                //command = @$"cat {filePath} | npx eslint --stdin --stdin-filename match_format.md"
                break;
            case ComponentType.InlineLatex:
            case ComponentType.BlockLatex:
                command = $"chktex";
                break;
            case ComponentType.Cpp:
                command = $"cpplint \"{filePath}\"";
                break;
            case ComponentType.C:
                command = $"cpplint \"{filePath}\"";
                break;
            case ComponentType.Python:
                command = $"flake8 \"{filePath}\"";
                break;
            case ComponentType.Sql:
                command = $"sqlfluff lint \"{filePath}\" --dialect ansi";
                break;
            default:
                break;
        }

        using (Process linter = new Process())
        {
            // Due to problem with installing chktex on Windows, we use Ubuntu on WSL and read from stdin 
            if (ComponentType.InlineLatex == componentType 
                || ComponentType.BlockLatex == componentType)
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
                linter.StandardInput.WriteLine(File.ReadAllText(filePath));
                linter.StandardInput.WriteLine("^D");
                linter.StandardInput.Close();

                // Waits until process terminates
                output = linter.StandardOutput.ReadToEnd();

                linter.WaitForExit();
            }
            else
            {
                linter.StartInfo.FileName = "cmd";

                // /c => Carries out the command specified by string and then terminates
                linter.StartInfo.Arguments = $"/c {command}";
                linter.StartInfo.RedirectStandardOutput = true;
                linter.StartInfo.UseShellExecute = false;
                linter.StartInfo.CreateNoWindow = true;
                linter.Start();
                
                output = linter.StandardOutput.ReadToEnd();

                linter.WaitForExit();
            }
        }

        File.Delete(filePath);

        return output;
    }
}