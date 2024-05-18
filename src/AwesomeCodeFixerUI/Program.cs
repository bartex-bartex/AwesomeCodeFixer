using AwesomeCodeFixerLibrary;
using System.Diagnostics;

namespace AwesomeCodeFixerUI;

internal class Program
{
    static void Main(string[] args)
    {
        string path = @"..\..\test\test_files\example_input.txt";

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        string content = File.ReadAllText(path);
        var output = Extractor.ExtractComponents(ref content);

        foreach (var item in output.Values)
        {
            Console.WriteLine(item);
            Console.WriteLine("-------------------------------------------------");
        }

        stopwatch.Stop();
        Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
    }
}
