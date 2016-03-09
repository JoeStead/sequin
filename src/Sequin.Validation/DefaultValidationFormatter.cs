namespace Sequin.Validation
{
    using Infrastructure;
    public class DefaultValidationFormatter : IValidationResultFormatter
    {
        public object Format(SequinValidationResult sequinValidationResult)
        {
            return sequinValidationResult.Result.ToString();
        }
    }
}
