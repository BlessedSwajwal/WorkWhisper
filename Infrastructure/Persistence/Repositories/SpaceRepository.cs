using Application.Common.Interface.Persistence;
using Domain.CompanySpace;
using Domain.CompanySpace.Entity;
using Domain.CompanySpace.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories;

public class SpaceRepository : ISpaceRepository
{
    private static readonly List<CompanySpace> _companySpace = new List<CompanySpace>();
    public void AddSpace(CompanySpace cs)
    {
        _companySpace.Add(cs);
    }

    public IReadOnlyCollection<PostId> GetAllPostId(CompanySpaceId id)
    {
        var company = GetSpaceById(id);
        var postIds = company.PostIds;
        return postIds;
    }

    public CompanySpace GetSpaceById(CompanySpaceId companySpaceId)
    {
        return _companySpace.FirstOrDefault(sp => sp.Id == companySpaceId);
    }
    public bool MemberExistsOrNot(CompanySpaceId spaceId, MemberId memberId)
    {
        var space = GetSpaceById(spaceId);

        return space.Members.Any(member => member == memberId);
    }
}
