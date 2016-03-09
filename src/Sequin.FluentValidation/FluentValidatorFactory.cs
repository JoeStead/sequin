namespace Sequin.FluentValidation
{
    using System;
    using System.Threading.Tasks;
    using global::FluentValidation;
    using global::FluentValidation.Results;
    using Validation;
    using Validation.Infrastructure;
    class FluentValidatorFactory : ISequinValidatorFactory
    {
        private readonly IValidatorFactory validatorFactory;

        public FluentValidatorFactory(IValidatorFactory validatorFactory)
        {
            this.validatorFactory = validatorFactory;
        }

        public Func<T, Task<SequinValidationResult>> GetValidator<T>()
        {
            var validator = validatorFactory.GetValidator<T>();
           return async command => (await validator.ValidateAsync(command)).ToSequinResult();
        }

        public object GenerateValidatorDocumentation<T>()
        {
            throw new System.NotImplementedException();
        }
    }

    public static class SequinExtensions
    {
        public static SequinValidationResult ToSequinResult(this ValidationResult result)
        {
            return new SequinValidationResult(result.IsValid, result);
        }
    }
}
