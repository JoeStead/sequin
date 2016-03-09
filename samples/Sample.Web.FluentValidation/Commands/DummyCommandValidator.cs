namespace Sample.Web.FluentValidation.Commands
{
    using global::FluentValidation;

    public class DummyCommandValidator : AbstractValidator<Commands.DummyCommand>
    {
        public DummyCommandValidator()
        {
            RuleFor(x => x.DummyProperty).GreaterThan(0);
            RuleFor(x => x.OtherDummyProperty).GreaterThan(0);
        }
    }
}