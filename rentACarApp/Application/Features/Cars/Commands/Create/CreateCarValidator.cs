using Application.Features.Cars.Validations;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Cars.Commands.Create
{
    public class CreateCarCommandValidator:AbstractValidator<CreateCarCommand>
    {
        public CreateCarCommandValidator()
        {
            RuleFor(c => c.ModelYear).GreaterThanOrEqualTo((short)2015);
            RuleFor(c=>c.MinFindeksCreditRate).GreaterThanOrEqualTo((short)0).LessThanOrEqualTo((short)1900);
            RuleFor(c=>c.Plate).NotEmpty().Must(CarCustomValidationRules.IsTurkeyPlate)
                .WithMessage("Plate is not a turkish plate");

        }
    }
}
