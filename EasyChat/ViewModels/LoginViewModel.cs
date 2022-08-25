using EasyChat.Attributes;

namespace EasyChat.ViewModels;

public class LoginViewModel
{
    [Name]
    public string Name { get; set; }

    [Password(Rules = 0, MinLength = 1)]
    public string Password { get; set; }
}