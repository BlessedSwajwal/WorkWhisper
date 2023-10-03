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
    private readonly WorkWhisperDbContext _context;

    public MemberRepository(WorkWhisperDbContext context)
    {
        _context = context;
    }

    public Member Add(Member member)
    {
        _context.Members.Add(member);
        return member;
    }

    public Member? GetMemberByEmail(string email)
    {
        return _context.Members.FirstOrDefault(m => m.Email == email);
    }

    public Member? GetMemberById(MemberId memberId)
    {
        return (_context.Members.FirstOrDefault(m =>m.Id == memberId));
    }

    public void UpdateMember(Member member)
    {
        _context.Members.Update(member);
    }
}
