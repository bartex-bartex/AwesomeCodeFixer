using System.Text.RegularExpressions;

namespace AwesomeCodeFixerLibrary;

internal static class ErrorManager
{
    public static List<ErrorModel> DeserializeIssues(string linterOutput, ComponentType componentType)
    {
        List<ErrorModel> output = new();

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

        return output;
    }

    private static List<ErrorModel> DeserializeLatexIssues(string linterOutput)
    {
        // TODO - Implement support also for errors not only warnings
        List<ErrorModel> output = new();

        // // Omit 2 first rows
        // List<string> errors = linterOutput.Split(Environment.NewLine).Skip(2).ToList();

        // // Consider 3 subsequent rows as single error (skip last to rows)
        // for (int i = 0; i < errors.Count - 2; i += 3)
        // {
        //     string[] chunks = errors[i].Split(':', 2);
        //     string rowChunk = chunks[0];
        //     string message = chunks[1];
        //     string columnChunk = errors[i + 2];

        //     // Get row
        //     string row = Regex.Match(rowChunk, @"line (\d+):").Groups[1].Value;

        //     // Get column
        //     string column = columnChunk.IndexOf("^").ToString();

        //     output.Add(new ErrorModel
        //     {
        //         Message = message,
        //         Row = row,
        //         Column = column
        //     });
        // }

        foreach(string line in linterOutput.Split(Environment.NewLine))
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

        return output;
    }

    private static List<ErrorModel> DeserializeSqlIssues(string linterOutput)
    {
        List<ErrorModel> output = new();

        return output;
    }
}