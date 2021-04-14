using CoreWebAPI.Models;
using FluentValidation;

namespace CoreWebAPI.Validators
{
    public class AttributeValidator : AbstractValidator<Attribute>
    {
        public AttributeValidator()
        {
            RuleFor(x => x.Key).NotEmpty().When(x => x.Value != null).WithMessage("Key cannot be empty when Value is present.");
            RuleFor(x => x.Value).NotEmpty().When(x => x.Key != null).WithMessage("Value cannot be empty when Key is present.");
        }
    }
}
