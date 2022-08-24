using System.ComponentModel.DataAnnotations;
using EasyChat.Attributes;

namespace EasyChat.Models.Api;

public class RegisterApiModel
{
    [Name]
    public string Name { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "incorrect email address")]
    [EmailAddress(ErrorMessage = "incorrect email address")]
    public string EmailAddress { get; set; }

    [Password(MinLength = 8, Rules = PasswordAttribute.PasswordRules.CapitalLetters | PasswordAttribute.PasswordRules.Numbers)]
    public string Password { get; set; }
}