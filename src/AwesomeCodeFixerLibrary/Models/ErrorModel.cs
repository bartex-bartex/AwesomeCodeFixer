namespace AwesomeCodeFixerLibrary; 

public class ErrorModel
{
    // TODO - change to int
    public string Row { get; set; }
    public string Column { get; set; }
    public string Message { get; set; } = string.Empty;
}