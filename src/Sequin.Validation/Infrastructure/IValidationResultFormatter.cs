namespace Sequin.Validation.Infrastructure
{
    public interface IValidationResultFormatter
    {
        object Format(SequinValidationResult sequinValidationResult);
    }
}