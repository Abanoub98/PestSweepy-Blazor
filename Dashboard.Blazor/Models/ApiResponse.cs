namespace Dashboard.Blazor.Models;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public string? RequestStatusCode { get; set; }
    public string? Error { get; set; }
    public List<string> Errors { get; set; } = new();
    public string? Message { get; set; }
    public T? Object { get; set; }
    public List<T>? ObjectsList { get; set; }
}
