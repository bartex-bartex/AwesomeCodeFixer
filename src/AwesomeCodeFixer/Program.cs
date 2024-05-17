using System.Diagnostics;

namespace AwesomeCodeFixer;

internal class Program
{
    static void Main(string[] args)
    {
        // string path = @"example_input.txt";
        string path = @"..\..\test\test_files\example_input.txt";

        // tu jest lista, której każdy element jest listą i składa się z:
        // [0] - numer linii w pliku, w którym zaczyna się dany komponent
        // [1] - co to za komponent
        // [2] - numer linii, w której komponent się kończy
        List<List<string>> components_info = Divide_Input.divide(path);
        // foreach (var y in components_info)
        // {
        //     Console.WriteLine(y[0] + " " + y[1] + " " + y[2]);
        // }

        // a tu dodatkowo lista:
        // [0] - w której linijce się zaczyna
        // [1] - cały tekst komponentu
        List<List<string>> components = Divide_Input.return_components(path, components_info);
        // foreach (var x in components)
        // {
        //     Console.WriteLine(x[0]);
        //     Console.WriteLine(x[1]);
        //     Console.WriteLine("xxxxxxxxxx");
        // }
        Divide_Input.get_feedback(path, components_info);
    }
}
