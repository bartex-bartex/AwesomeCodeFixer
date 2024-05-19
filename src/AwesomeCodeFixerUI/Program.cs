using AwesomeCodeFixerLibrary;
using System.Diagnostics;

namespace AwesomeCodeFixerUI;

internal class Program
{
    static void Main(string[] args)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        string path = @"..\..\test\test_files\example_input.txt";
        string content = File.ReadAllText(path);

        //string formattedContent = ExtensionManager.FormatCode(content);
        string lintOutput = ExtensionManager.LintCode(content);

        //System.Console.WriteLine(formattedContent);
        System.Console.WriteLine(lintOutput);

        stopwatch.Stop();
        Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
    }
}
