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

        string exampleMarkdownRule = "markdownlint/md001";
        int ruleLength = exampleMarkdownRule.Length;

        // Remove rule from each line
        List<string> lines = linterOutput
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Where(x => Regex.IsMatch(x, @"^\d+:\d+"))
                .ToList();

        foreach (var line in lines)
        {
            string[] chunks = line.Split(new string[] {"  error  ", "  warning  "},
                 StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            
            string[] position = chunks[0].Split(':');
            string row = position[0];
            string col = position[1];

            // remove markdown rule from message
            string message = chunks[1].Substring(0, chunks[1].Length - ruleLength).Trim();

            int.TryParse(row, out int rowInt);
            int.TryParse(col, out int colInt);

            output.Add(new ErrorModel
            {
                Row = rowInt,
                Column = colInt,
                Message = message,
                Severity = "warning"
            });
        }

        return output;
    }

    private static List<ErrorModel> DeserializeLatexIssues(string linterOutput)
    {
        List<ErrorModel> output = new();

        List<string> lines = linterOutput.Trim()
            .Split("|n|", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();

        foreach(string line in lines)
        {
            string[] chunks =  line.Split(':', 4);

            int.TryParse(chunks[0], out int rowInt);
            int.TryParse(chunks[1], out int colInt);

            output.Add(new ErrorModel
            {
                Row = rowInt,
                Column = colInt,
                Message = chunks[3],
                Severity = chunks[2]
            });
        }

        return output;
    }

    private static List<ErrorModel> DeserializeCppIssues(string linterOutput)
    {
        List<ErrorModel> output = new();

        List<string> lines = linterOutput
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => x[0] == '/')
                .ToList();

        // Setup for Linux
        foreach (string line in lines)
        {
            string[] chunks = line.Split(':', 5);

            int.TryParse(chunks[1], out int rowInt);
            int.TryParse(chunks[2], out int colInt);

            output.Add(new ErrorModel
            {
                Row = rowInt,
                Column = colInt,
                Message = chunks[4].Trim(),
                Severity = chunks[3].Trim()
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

            string severity;
            if (chunks[3].TrimStart().StartsWith('E'))
            {
                severity = "error";
            }
            else
            {
                severity = "warning";
            }

            output.Add(new ErrorModel
            {
                Row = rowInt,
                Column = colInt,
                Message = chunks[3].Trim(),
                Severity = severity
            });
        }

        return output;
    }

    private static List<ErrorModel> DeserializeSqlIssues(string linterOutput)
    {
        List<ErrorModel> output;

        output = JsonConvert.DeserializeObject<List<ErrorModel>>(linterOutput);

        return output;
    }
}