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

            int.TryParse(row, out int rowInt);
            int.TryParse(col, out int colInt);

            output.Add(new ErrorModel
            {
                Row = rowInt,
                Column = colInt,
                Message = message
            });
        }

        return output;
    }

    private static List<ErrorModel> DeserializeLatexIssues(string linterOutput)
    {
        List<ErrorModel> output = new();

        foreach(string line in linterOutput.Trim().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
        {
            string[] chunks =  line.Split(' ', 3);

            int.TryParse(chunks[0], out int rowInt);
            int.TryParse(chunks[1], out int colInt);

            output.Add(new ErrorModel
            {
                Row = rowInt,
                Column = colInt,
                Message = chunks[2]
            });
        }

        return output;
    }

    private static List<ErrorModel> DeserializeCppIssues(string linterOutput)
    {
        List<ErrorModel> output = new();

        // Setup for Windows
        foreach (string line in linterOutput
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => char.IsUpper(x[0]) && x.Length > 1 && x[1] == ':'))
        {
            string[] chunks = line.Split(':', 5);

            int.TryParse(chunks[2], out int rowInt);
            int.TryParse(chunks[3], out int colInt);

            output.Add(new ErrorModel
            {
                Row = rowInt,
                Column = colInt,
                Message = chunks[4].Trim()
            });
        }

        return output;
    }

    private static List<ErrorModel> DeserializeCIssues(string linterOutput)
    {
        return DeserializeCppIssues(linterOutput);
    }

    private static List<ErrorModel> DeserializePythonIssues(string linterOutput)
    {
        List<ErrorModel> output = new();

        foreach (string line in linterOutput.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
        {
            string[] chunks = line.Split(':', 4);

            int.TryParse(chunks[1], out int rowInt);
            int.TryParse(chunks[2], out int colInt);

            output.Add(new ErrorModel
            {
                Row = rowInt,
                Column = colInt,
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