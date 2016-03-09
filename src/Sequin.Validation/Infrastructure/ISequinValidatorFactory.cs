namespace Sequin.Validation.Infrastructure
{
    using System;
    using System.Threading.Tasks;

    public interface ISequinValidatorFactory
    {
        Func<T, Task<SequinValidationResult>> GetValidator<T>();
        object GenerateValidatorDocumentation<T>();
    }
}