namespace Sequin.Validation.Middleware
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.ExceptionServices;
    using System.Threading.Tasks;
    using Extensions;
    using Infrastructure;
    using Microsoft.Owin;

    public class ValidateCommand : OwinMiddleware
    {
        private readonly ISequinValidatorFactory sequinValidatorFactory;
        private readonly IValidationResultFormatter validationResultFormatter;

        public ValidateCommand(OwinMiddleware next, ISequinValidatorFactory sequinValidatorFactory) : this(next, sequinValidatorFactory, new DefaultValidationFormatter()) { }

        public ValidateCommand(OwinMiddleware next, ISequinValidatorFactory sequinValidatorFactory, IValidationResultFormatter validationResultFormatter) : base(next)
        {
            this.sequinValidatorFactory = sequinValidatorFactory;
            this.validationResultFormatter = validationResultFormatter;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var command = context.GetCommand();
            var validationResult = await Validate(command);

            if (validationResult != null && validationResult.IsValid)
            {
                await Next.Invoke(context);
            }
            else if (validationResult != null)
            {
                WriteErrorResponse(context, validationResult);
            }
            else
            {
                context.Response.BadRequest("No command validator registered for the specified command type.");
            }
        }

        private void WriteErrorResponse(IOwinContext context, SequinValidationResult sequinValidationResult)
        {
            var errors = validationResultFormatter.Format(sequinValidationResult);
            
            context.Response.BadRequest("The command contained validation errors.");
            context.Response.Json(errors);
        }

        private Task<SequinValidationResult> Validate(object command)
        {
            var commandType = command.GetType();

            try
            {
                Expression<Action<object>> expression = x => ExecuteValidator(x);
                var methodCallExpression = (MethodCallExpression)expression.Body;
                var methodInfo = methodCallExpression.Method.GetGenericMethodDefinition().MakeGenericMethod(commandType);

                var result = (Task<SequinValidationResult>)methodInfo.Invoke(this, new[] { command });
                return result;

            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }

            return null;
        }

        private Task<SequinValidationResult> ExecuteValidator<T>(T command)
        {
            var validator = sequinValidatorFactory.GetValidator<T>();
            if (validator != null)
            {
                return validator.Invoke(command);
            }

            return Task.FromResult<SequinValidationResult>(null);
        }
    }
}