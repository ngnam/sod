using Microsoft.AspNetCore.Mvc.ModelBinding;
using StoreOrder.WebApplication.Data.Wrappers;
using System.Collections.Generic;
using System.Linq;

namespace StoreOrder.WebApplication.Extensions
{
    public static class ModelStateExtension
    {
        public static IEnumerable<ValidationError> AllErrors(this ModelStateDictionary modelState)
        {
            return modelState.Keys.SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage))).ToList();
        }
    }
}
