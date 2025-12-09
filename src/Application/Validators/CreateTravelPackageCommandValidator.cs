using Application.Features.TravelPackages.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class CreateTravelPackageCommandValidator : AbstractValidator<CreateTravelPackageCommand>
    {
        public CreateTravelPackageCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

            RuleFor(x => x.Destination)
                .NotEmpty().WithMessage("Destination is required")
                .MaximumLength(500).WithMessage("Destination must not exceed 500 characters");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");

            RuleFor(x => x.MaxCapacity)
                .GreaterThan(0).WithMessage("Max capacity must be greater than 0");

            RuleFor(x => x.StartDate)
                .GreaterThan(DateTime.Today).WithMessage("Start date must be in the future");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date");
        }
    }
}
