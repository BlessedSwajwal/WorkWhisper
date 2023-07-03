using Application.Common.Interface.Persistence;
using Domain.CompanySpace.Entity;
using Domain.CompanySpace.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories;

public class MemberRepository : IMemberRepository
{
    private readonly List<Member> _members = new List<Member>();
    public Member Add(Member member)
    {
        _members.Add(member);
        return member;
    }

    public Member? GetMemberByEmail(string email)
    {
        return _members.FirstOrDefault(m => m.Email == email);      
    }

    public Member? GetMemberById(MemberId memberId)
    {
        return _members.FirstOrDefault(m => m.Id == memberId);
    }
}
