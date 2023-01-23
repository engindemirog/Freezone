using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Brands.Commands.Create
{
    public class CreateBrandCommandValidator:AbstractValidator<CreateBrandCommand>
    {
        public CreateBrandCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.Name).MinimumLength(2);
            RuleFor(c=>c.Name).MaximumLength(20).When(StartsWithA);

        }

        private bool StartsWithA(CreateBrandCommand arg)
        {
            return arg.Name.ToLower().StartsWith("a");
        }
    }
}
