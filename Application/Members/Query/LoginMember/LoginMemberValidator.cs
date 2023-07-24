using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Members.Query.LoginMember;

public class LoginMemberValidator : AbstractValidator<LoginMemberQuery>
{
    public LoginMemberValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Can not be empty")
            .EmailAddress();

        RuleFor(x => x.Password).NotEmpty();
    }
}
