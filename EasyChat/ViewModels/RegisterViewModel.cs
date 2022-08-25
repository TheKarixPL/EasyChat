using System.ComponentModel.DataAnnotations;
using EasyChat.Attributes;

namespace EasyChat.ViewModels;

public class RegisterViewModel
{
    [Name]
    public string Name { get; set; }

    [Password(MinLength = 8, Rules = PasswordAttribute.PasswordRules.Numbers | PasswordAttribute.PasswordRules.CapitalLetters | PasswordAttribute.PasswordRules.SpecialCharacters)]
    public string Password { get; set; }

    [Required(AllowEmptyStrings = false)]
    [EmailAddress]
    public string EmailAddress { get; set; }
}