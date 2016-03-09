namespace Sequin.FluentValidation
{
    using System.Linq;
    using global::FluentValidation.Results;
    using Validation;
    using Validation.Infrastructure;

    internal class DefaultDictionaryFormatter : IValidationResultFormatter
    {
        public object Format(SequinValidationResult validationResult)
        {
            var result = (ValidationResult) validationResult.Result;
            var errors = result.Errors
                                         .GroupBy(k => k.PropertyName)
                                         .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage));

            return errors;
        }
    }
}
