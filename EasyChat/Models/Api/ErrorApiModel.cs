namespace EasyChat.Models.Api;

public class ErrorApiModel
{
    public List<string> ValidationErrors { get; set; } = new List<string>();
    public string Error { get; set; } = string.Empty;
}