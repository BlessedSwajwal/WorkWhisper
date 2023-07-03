using Domain.CompanySpace.Entity;
using Domain.CompanySpace.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interface.Persistence;

public interface IMemberRepository
{
    Member Add(Member member);
    Member? GetMemberByEmail(string email);
    Member? GetMemberById(MemberId memberId);
}