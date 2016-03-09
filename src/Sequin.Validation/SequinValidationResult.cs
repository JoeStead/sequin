namespace Sequin.Validation
{
    public class SequinValidationResult
    {
        public bool IsValid { get; }
        public object Result { get; }

        public SequinValidationResult(bool isValid, object result)
        {
            IsValid = isValid;
            Result = result;
        }
    }
}
