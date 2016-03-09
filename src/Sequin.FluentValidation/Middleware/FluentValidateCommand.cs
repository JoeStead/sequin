namespace Sequin.FluentValidation.Middleware
{
    using global::FluentValidation;
    using Microsoft.Owin;
    using Validation.Infrastructure;
    using Validation.Middleware;

    public class FluentValidateCommand : ValidateCommand
    {

        public FluentValidateCommand(OwinMiddleware next, IValidatorFactory validatorFactory) : this(next, new FluentValidatorFactory(validatorFactory), new DefaultDictionaryFormatter()) { }
        public FluentValidateCommand(OwinMiddleware next, IValidatorFactory validatorFactory, IValidationResultFormatter resultFormatter) : this(next, new FluentValidatorFactory(validatorFactory), resultFormatter) { }

        private FluentValidateCommand(OwinMiddleware next, ISequinValidatorFactory sequinValidatorFactory, IValidationResultFormatter validationResultFormatter) : base(next, sequinValidatorFactory, validationResultFormatter){ }
    }
}