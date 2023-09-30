using Domain.Common.Models;
using Domain.CompanySpace.ValueObjects;
using Domain.Member.ValueObjects;
using Domain.Post.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Member;

public sealed class Member : AggregateRoot<MemberId, Guid>
{
    public string Name { get; private set; }
    public CompanySpaceId CompanySpaceId { get; private set; }
    private List<PostId> _postIds = new List<PostId>();
    public IReadOnlyList<PostId> PostIds => _postIds.AsReadOnly();

    public string Email { get; private set; }
    public string Password { get; private set; }

    private Member(MemberId id, string name, string email, string password, CompanySpaceId companySpaceId) : base(id)
    {
        Name = name;
        Email = email;
        Password = password;
        CompanySpaceId = companySpaceId;
    }

    public static Member Create(string name, string email, string password, CompanySpaceId companySpaceId)
    {
        return new(MemberId.CreateUnique(), name, email, password, companySpaceId);
    }

    public void AddPost(Guid postId)
    {
        _postIds.Add(PostId.Create(postId));
    }

    private Member() { }
}
