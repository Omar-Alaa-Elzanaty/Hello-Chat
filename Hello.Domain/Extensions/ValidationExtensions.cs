using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace Hello.Domain.Extensions
{
    public static class ValidationExtensions
    {
        public static Dictionary<string, List<string>> GetErrorsDictionary(this List<ValidationFailure> validationFailures)
        {
            Dictionary<string, List<string>> errors = [];

            validationFailures.ForEach(a =>
            {
                if (errors.ContainsKey(a.PropertyName))
                {
                    errors[a.PropertyName].Add(a.ErrorMessage);
                }
                else
                {
                    errors.Add(a.PropertyName, [a.ErrorMessage]);
                }
            });

            return errors;
        }
        
        public static Dictionary<string, List<string>> GetErrorsDictionary(this List<IdentityError> validationFailures)
        {
            Dictionary<string, List<string>> errors = [];

            validationFailures.ForEach(a =>
            {
                if (errors.ContainsKey(a.Code))
                {
                    errors[a.Code].Add(a.Description);
                }
                else
                {
                    errors.Add(a.Code, [a.Description]);
                }
            });

            return errors;
        }
    }
}
