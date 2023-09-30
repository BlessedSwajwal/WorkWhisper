using Domain.Common.Models;
using Domain.CompanySpace.ValueObjects;
using Domain.Member.ValueObjects;
using Domain.Post.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CompanySpace;

public class CompanySpace : AggregateRoot<CompanySpaceId, Guid>
{
    public string Name { get; private set; }
    private List<MemberId> _members = new List<MemberId>();
    public IReadOnlyCollection<MemberId> Members => _members.AsReadOnly();

    private List<PostId> _postIds = new List<PostId>();
    public IReadOnlyCollection<PostId> PostIds => _postIds.AsReadOnly();

    private CompanySpace(CompanySpaceId companySpaceId, string name) : base(companySpaceId)
    {
        Name = name;
    }

    public static CompanySpace Create(string name)
    {
        return new(CompanySpaceId.CreateUnique(),
                   name);
    }

    public void AddMember(Guid memberId)
    {
        _members.Add(MemberId.Create(memberId));
    }

    public void AddPost(Guid postId)
    {
        _postIds.Add(PostId.Create(postId));
    }


    private CompanySpace() { }
}
