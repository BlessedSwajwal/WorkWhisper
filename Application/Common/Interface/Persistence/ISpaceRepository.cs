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

namespace Application.Common.Interface.Persistence;

public interface ISpaceRepository
{
    void AddSpace(CompanySpace space);
    CompanySpace GetSpaceById(CompanySpaceId companySpaceId);
    
    bool MemberExistsOrNot(CompanySpaceId spaceId, MemberId memberId);

    IReadOnlyCollection<PostId> GetAllPostId(CompanySpaceId id);
    List<CompanySpaceResult> GetAllSpaces();
    void AddMember(Member member, CompanySpace space);
}
