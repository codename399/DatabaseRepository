using DatabaseRepository.Common.Utilities;
using FluentValidation;

namespace DatabaseRepository.Validators
{
    public class IdValidator : AbstractValidator<string>
    {
        public IdValidator()
        {
            RuleFor(x => x)
                    .NotNull()
            .Must(Util.IsValidGuid);
        }
    }
}
