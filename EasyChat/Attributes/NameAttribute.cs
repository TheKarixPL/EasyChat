using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EasyChat.Attributes;

public class NameAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is string)
        {
            var name = (string)value;
            if (name.Length != 0)
            {
                if (name.Length < 50)
                {
                    if (!Regex.IsMatch(name, @"[^A-Za-z1-9_]"))
                        return ValidationResult.Success;
                    else
                        return new ValidationResult("value is not correct");
                }
                else
                {
                    return new ValidationResult("name must be lesser than 50");
                }
            }
            else
            {
                return new ValidationResult("value is empty");
            }
        }
        else
        {
            return new ValidationResult("value is not string");
        }
    }
}