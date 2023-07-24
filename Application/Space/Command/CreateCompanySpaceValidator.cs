using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Space.Command;

public class CreateCompanySpaceValidator : AbstractValidator<CreateCompanySpaceCommand>
{
    public CreateCompanySpaceValidator()
    {
       RuleFor(x => x.Name).NotEmpty();
    }
}
