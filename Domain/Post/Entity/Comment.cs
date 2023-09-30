using Domain.Common.Models;
using Domain.Member.ValueObjects;
using Domain.Post.ValueObjects;

namespace Domain.Post.Entity;

public sealed class Comment : Entity<CommentId>
{
    public string Text { get; private set; }
    public MemberId CommentorId { get; private set; }
    private List<MemberId> _upvotingMemberIds = new List<MemberId>();
    public IReadOnlyList<MemberId> UpvotingMemberIds => _upvotingMemberIds.AsReadOnly();

    private Comment(CommentId id, string text, MemberId commenterId) : base(id)
    {
        Text = text;
        CommentorId = commenterId;
    }

    public static Comment Create(string text, MemberId commenterId)
    {
        return new(CommentId.CreateUnique(), text, commenterId);
    }

    public void Upvote(MemberId memberId)
    {
        _upvotingMemberIds.Add(memberId);
    }

    private Comment() { }
}
