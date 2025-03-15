using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Extension
{
    public static class FluentValidationExtension
    {
        public static Dictionary<string, string[]> ObterErros(this FluentValidation.Results.ValidationResult validationResult)
        {
            var errors = new Dictionary<string, string[]>();
            foreach (var error in validationResult.Errors)
            {
                if (!errors.ContainsKey(error.PropertyName))
                    errors.Add(error.PropertyName, new string[] { error.ErrorMessage });
                else
                {
                    var messages = errors[error.PropertyName].ToList();
                    messages.Add(error.ErrorMessage);
                    errors[error.PropertyName] = messages.ToArray();
                }
            }
            return errors;
        }
    }
}
