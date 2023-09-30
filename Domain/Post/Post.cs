using Domain.Common.Models;
using Domain.CompanySpace.ValueObjects;
using Domain.Member.ValueObjects;
using Domain.Post.Entity;
using Domain.Post.ValueObjects;

namespace Domain.Post;

public sealed class Post : AggregateRoot<PostId, Guid>
{
    public string Title { get; private set; }
    public string Body { get; private set; }
    public bool IsEdited { get; private set; } = false;
    public bool IsPrivate { get; private set; }

    public MemberId OwnerId { get; private set; }
    public CompanySpaceId SpaceId { get; private set; }

    private List<Comment> _comments = new List<Comment>();
    public IReadOnlyList<Comment> Comments => _comments.AsReadOnly();

    private List<MemberId> _upvotingMemberIds = new List<MemberId>();
    public IReadOnlyList<MemberId> UpvotingMemberIds => _upvotingMemberIds.AsReadOnly();

    private Post(PostId id, string title, string body, CompanySpaceId spaceId, MemberId ownerId, bool isPrivate)
        : base(id)
    {
        Title = title;
        Body = body;
        SpaceId = spaceId;
        OwnerId = ownerId;
        IsPrivate = isPrivate;
    }

    public static Post Create(string title, string body, CompanySpaceId spaceId, MemberId memberId, bool isPrivate)
    {
        return new(PostId.CreateUnique(), title, body, spaceId, memberId, isPrivate);
    }

    public void AddComment(Comment comment)
    {
        _comments.Add(comment);
    }

    public void Upvote(MemberId memberId)
    {
        _upvotingMemberIds.Add(memberId);
    }

    public void EditPost(string body)
    {
        IsEdited = true;
        Body = body;
    }

    private Post() { }
}
