using Application.Features.Bookings.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingCommandValidator()
        {
            RuleFor(x => x.TravelPackageId)
                .NotEmpty().WithMessage("Travel package ID is required");

            RuleFor(x => x.NumberOfTravelers)
                .GreaterThan(0).WithMessage("Number of travelers must be greater than 0")
                .LessThanOrEqualTo(20).WithMessage("Maximum 20 travelers per booking");

            RuleFor(x => x.Travelers)
                .NotEmpty().WithMessage("Traveler information is required")
                .Must((command, travelers) => travelers.Count == command.NumberOfTravelers)
                .WithMessage("Number of travelers must match traveler details provided");

            RuleForEach(x => x.Travelers).ChildRules(traveler =>
            {
                traveler.RuleFor(t => t.FirstName)
                    .NotEmpty().WithMessage("Traveler first name is required");

                traveler.RuleFor(t => t.LastName)
                    .NotEmpty().WithMessage("Traveler last name is required");

                traveler.RuleFor(t => t.PassportNumber)
                    .NotEmpty().WithMessage("Passport number is required");

                traveler.RuleFor(t => t.PassportExpiry)
                    .GreaterThan(DateTime.Today.AddMonths(6))
                    .WithMessage("Passport must be valid for at least 6 months");
            });
        }
    }
}
