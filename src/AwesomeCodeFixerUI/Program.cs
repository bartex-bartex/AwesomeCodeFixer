using AwesomeCodeFixerLibrary;
using System.Diagnostics;

namespace AwesomeCodeFixerUI;

internal class Program
{
    static void Main(string[] args)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        string path = @"../../test/test_files/example_input.txt";
        string content = File.ReadAllText(path);

        string formattedContent = ExtensionManager.FormatCode(content);
        System.Console.WriteLine(formattedContent);

        // var lintOutput = ExtensionManager.LintCode(content);

        // foreach (var error in lintOutput)
        // {
        //     System.Console.WriteLine($"{error.Row}:{error.Column} | {error.Message}");
        // }

        stopwatch.Stop();
        Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
    }
}
