using Application.Common.Interface.Persistence;
using Application.Space.Query.GetAllSpaces;
using Domain.CompanySpace;
using Domain.CompanySpace.ValueObjects;
using Domain.Member;
using Domain.Member.ValueObjects;
using Domain.Post.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.EFCoreRepositories;

public class SpaceRepository : ISpaceRepository
{
    private readonly WorkWhisperDbContext _context;

    public SpaceRepository(WorkWhisperDbContext context)
    {
        _context = context;
    }

    public void AddMember(Member member, CompanySpace space)
    {
        space.AddMember(member.Id.Value);
        _context.CompanySpaces.Attach(space);
    }

    public void AddSpace(CompanySpace space)
    {
        _context.CompanySpaces.Add(space);
    }

    public IReadOnlyCollection<PostId> GetAllPostId(CompanySpaceId id)
    {
        var space = _context.CompanySpaces.FirstOrDefault(cs => cs.Id == id)!;
        var postIds = space.PostIds;
        return postIds;
    }

    public List<CompanySpaceResult> GetAllSpaces()
    {
        return _context.CompanySpaces.Select(sp => new CompanySpaceResult(sp.Id.Value, sp.Name)).ToList();
    }

    public CompanySpace GetSpaceById(CompanySpaceId companySpaceId)
    {
        return _context.CompanySpaces.FirstOrDefault(cs => cs.Id == companySpaceId);
    }

    public bool MemberExistsOrNot(CompanySpaceId spaceId, MemberId memberId)
    {
        return _context.CompanySpaces.FirstOrDefault(cs => cs.Id == spaceId)!.Members.Contains(memberId);
    }

    public void UpdateSpace(CompanySpace space)
    {
        _context.CompanySpaces.Update(space);
    }
}
