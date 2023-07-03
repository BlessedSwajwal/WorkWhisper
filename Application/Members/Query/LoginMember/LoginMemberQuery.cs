using Application.Members.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Members.Query.LoginMember;

public record LoginMemberQuery(string Email, string Password) : IRequest<MemberResult>;