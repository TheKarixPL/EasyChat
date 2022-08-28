using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EasyChat.Attributes;

public class PasswordAttribute : ValidationAttribute
{
    [Flags]
    public enum PasswordRules
    {
        CapitalLetters = 1,
        Numbers = 2,
        SpecialCharacters = 4
    }
    
    private int _minLength = 8;
    
    public int MinLength
    {
        get => _minLength;
        set => _minLength = value > 0 ? value : throw new ArgumentException("MinLength must be greater than 0");
    }

    public PasswordRules Rules { get; set; } = 0;

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is string)
        {
            var pwd = (string)value;
            if (pwd.Length >= MinLength)
            {
                if (Rules.HasFlag(PasswordRules.CapitalLetters) && pwd == pwd.ToLower())
                    return new ValidationResult(validationContext.DisplayName + " must contain capital letters");
                if (Rules.HasFlag(PasswordRules.Numbers) && !Regex.IsMatch(pwd, @"[1-9]"))
                    return new ValidationResult(validationContext.DisplayName + " must contain numbers");
                if (Rules.HasFlag(PasswordRules.SpecialCharacters) && !Regex.IsMatch(pwd, @"[!@#$%^&*()+\-*\/_\[\]]"))
                    return new ValidationResult(validationContext.DisplayName + " must contain special characters");
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult($"{validationContext.DisplayName} is lesser than {MinLength} characters");
            }
        }
        else
        {
            return new ValidationResult(validationContext.DisplayName + " is not string");
        }
    }
}