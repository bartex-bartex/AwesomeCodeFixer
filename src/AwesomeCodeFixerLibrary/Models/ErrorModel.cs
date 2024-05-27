using Newtonsoft.Json;

namespace AwesomeCodeFixerLibrary; 

public class ErrorModel : IComparable<ErrorModel>
{
    // TODO - change to int
    [JsonProperty("start_line")]
    public int Row { get; set; }

    [JsonProperty("start_column")]
    public int Column { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; } = string.Empty;

    [JsonProperty("annotation_level")]
    public string Severity { get; set; } = string.Empty;

    public int CompareTo(ErrorModel? other)
    {
        if (other == null) return 1;

        int rowComparison = Row.CompareTo(other.Row);
        if (rowComparison != 0) return rowComparison;

        return Column.CompareTo(other.Column);
    }
}