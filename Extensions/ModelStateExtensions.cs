using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Extensions;

public static class ModelStateExtensions
{
    public static List<string> GetErrors(this ModelStateDictionary modelState)
    {
        var result = new List<string>();
        foreach (var item in modelState.Values)
            result.AddRange(item.Errors.Select(x => x.ErrorMessage));

        return result;
    }
}
