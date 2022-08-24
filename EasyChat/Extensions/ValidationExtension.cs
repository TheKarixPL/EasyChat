using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EasyChat.Extensions;

public static class ValidationExtension
{
    public static IEnumerable<string> GetValidationErrors(this ModelStateDictionary modelStateDictionary)
        => modelStateDictionary.Select(ms => ms.Value)
            .Where(ms => ms.ValidationState == ModelValidationState.Invalid).SelectMany(ms => ms.Errors).Select(e => e.ErrorMessage);
}