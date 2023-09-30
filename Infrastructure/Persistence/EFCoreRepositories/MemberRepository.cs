using Application.Common.Interface.Persistence;
using Domain.Member;
using Domain.Member.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.EFCoreRepositories;

public class MemberRepository : IMemberRepository
{
    public Member Add(Member member)
    {
        throw new NotImplementedException();
    }

    public Member? GetMemberByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public Member? GetMemberById(MemberId memberId)
    {
        throw new NotImplementedException();
    }
}
