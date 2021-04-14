using CoreWebAPI.Data;
using CoreWebAPI.Models;
using FluentValidation;
using System;
using System.Linq;

namespace CoreWebAPI.Validators
{
    public class BatchValidator : AbstractValidator<BatchViewModel>
    {
        private BatchContext _context;

        public BatchValidator(BatchContext context)
        {
            _context = context;
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.BusinessUnit).NotEmpty().WithMessage("Business unit cannot be null or blank");
            RuleFor(x => x.ExpiryDate).NotEmpty();
            //RuleFor(x => x.ExpiryDate).GreaterThanOrEqualTo(r => DateTime.Now).WithMessage("Batch Expired (Expiry Date Ended)");
            RuleForEach(x => x.Attributes).SetValidator(new AttributeValidator());
            RuleFor(x => x.BusinessUnit).Must(IsBusinessUnitExists).WithMessage("Not a valid Business Unit.");
        }

        private bool IsBusinessUnitExists(string arg)
        {
            var BusinessUnit = _context.BusinessUnits.Where(x => x.BusinessUnitName.ToUpper() == arg.ToUpper()).SingleOrDefault();

            if (BusinessUnit != null)
                return true;
            else
                return false;
        }
    }
}
