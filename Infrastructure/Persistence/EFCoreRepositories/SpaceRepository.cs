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
    public void AddMember(Member member, CompanySpace space)
    {
        throw new NotImplementedException();
    }

    public void AddSpace(CompanySpace space)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<PostId> GetAllPostId(CompanySpaceId id)
    {
        throw new NotImplementedException();
    }

    public List<CompanySpaceResult> GetAllSpaces()
    {
        throw new NotImplementedException();
    }

    public CompanySpace GetSpaceById(CompanySpaceId companySpaceId)
    {
        throw new NotImplementedException();
    }

    public bool MemberExistsOrNot(CompanySpaceId spaceId, MemberId memberId)
    {
        throw new NotImplementedException();
    }
}
