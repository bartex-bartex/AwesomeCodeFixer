using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace AwesomeCodeFixerLibrary;

internal static class ErrorManager
{
    public static List<ErrorModel> DeserializeIssues(string linterOutput, ComponentType componentType)
    {
        List<ErrorModel> output = new();

        if (string.IsNullOrEmpty(linterOutput)) return output;

        switch (componentType)
        {
            case ComponentType.Markdown:
                output = DeserializeMarkdownIssues(linterOutput);
                break;
            case ComponentType.InlineLatex:
            case ComponentType.BlockLatex:
                output = DeserializeLatexIssues(linterOutput);
                break;
            case ComponentType.CppCodeBlock:
                output = DeserializeCppIssues(linterOutput);
                break;
            case ComponentType.CCodeBlock:
                output = DeserializeCIssues(linterOutput);
                break;
            case ComponentType.PythonCodeBlock:
                output = DeserializePythonIssues(linterOutput);
                break;
            case ComponentType.SqlCodeBlock:
                output = DeserializeSqlIssues(linterOutput);
                break;
            default:
                break;
        }

        return output;
    }

    private static List<ErrorModel> DeserializeMarkdownIssues(string linterOutput)
    {
        List<ErrorModel> output = new();

        List<string> lines = linterOutput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList(); 

        // Skip frist 2 and last 5 rows
        for (int i = 2; i < lines.Count - 5; i ++)
        {
            string[] chunks = lines[i].TrimStart().Split("  ", 4);
            string row = chunks[0].Split(':')[0];
            string col = chunks[0].Split(':')[1];

            string message = chunks[2];

            output.Add(new ErrorModel
            {
                Row = row,
                Column = col,
                Message = message
            });
        }

        return output;
    }

    private static List<ErrorModel> DeserializeLatexIssues(string linterOutput)
    {
        List<ErrorModel> output = new();

        foreach(string line in linterOutput.Split('\n'))
        {
            string[] chunks =  line.Split(' ', 3);

            output.Add(new ErrorModel
            {
                Row = chunks[0],
                Column = chunks[1],
                Message = chunks[2]
            });
        }

        return output;
    }

    private static List<ErrorModel> DeserializeCppIssues(string linterOutput)
    {
        List<ErrorModel> output = new();

        return output;
    }

    private static List<ErrorModel> DeserializeCIssues(string linterOutput)
    {
        List<ErrorModel> output = new();

        return output;
    }

    private static List<ErrorModel> DeserializePythonIssues(string linterOutput)
    {
        List<ErrorModel> output = new();

        foreach (string line in linterOutput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
        {
            string[] chunks = line.Split(':', 4);

            output.Add(new ErrorModel
            {
                Row = chunks[1],
                Column = chunks[2],
                Message = chunks[3]
            });
        }

        return output;
    }

    private static List<ErrorModel> DeserializeSqlIssues(string linterOutput)
    {
        List<ErrorModel> output = new();

        output = JsonConvert.DeserializeObject<List<ErrorModel>>(linterOutput);

        return output;
    }
}