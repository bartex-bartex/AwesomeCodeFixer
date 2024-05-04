using System.Diagnostics;

namespace AwesomeCodeFixer;

internal class Program
{
    static void Main(string[] args)
    {
        // string tempFolderWithFiles = "../../../../../../src/test_files"; // This is for Visual Studio (not code)
        string tempFolderWithFiles = "../../../src/test_files";

        // string markdown = Linter.Lint(File.ReadAllText(Path.Combine(tempFolderWithFiles, "a.md")), ComponentType.Markdown);
        string latex = Linter.Lint(File.ReadAllText(Path.Combine(tempFolderWithFiles, "c.tex")), ComponentType.InlineLatex);
        // string cpp = Linter.Lint(File.ReadAllText(Path.Combine(tempFolderWithFiles, "e.cpp")), ComponentType.Cpp);
        // string c = Linter.Lint(File.ReadAllText(Path.Combine(tempFolderWithFiles, "i.c")), ComponentType.C);
        // string python = Linter.Lint(File.ReadAllText(Path.Combine(tempFolderWithFiles, "g.py")), ComponentType.Python);
        // string sql = Linter.Lint(File.ReadAllText(Path.Combine(tempFolderWithFiles, "h.sql")), ComponentType.Sql);
        
        Console.WriteLine(latex);

        // string path = @"example_input.txt";

        // // tu jest lista, której każdy element jest listą i składa się z:
        // // [0] - numer linii w pliku, w którym zaczyna się dany komponent
        // // [1] - co to za komponent
        // // [2] - numer linii, w której komponent się kończy
        // List<List<string>> components_info = Divide_Input.divide(path);
        // foreach (var y in components_info)
        // {
        //     Console.WriteLine(y[0] + " " + y[1] + " " + y[2]);
        // }

        // // a tu dodatkowo lista:
        // // [0] - w której linijce się zaczyna
        // // [1] - cały tekst komponentu
        // List<List<string>> components = Divide_Input.return_components(path, components_info);
        // foreach (var x in components)
        // {
        //     Console.WriteLine(x[0]);
        //     Console.WriteLine(x[1]);
        //     Console.WriteLine("xxxxxxxxxx");
        // }
    }
}
