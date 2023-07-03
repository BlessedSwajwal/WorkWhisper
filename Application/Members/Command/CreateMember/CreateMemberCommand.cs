using Application.Members.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Members.Command.CreateMember;

public record CreateMemberCommand
(
    string Name,
    string Email,
    string Password,
    Guid CompanySpaceId
) : IRequest<MemberResult>;
