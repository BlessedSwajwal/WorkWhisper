using Application.Common.Exceptions;
using Application.Common.Interface.Persistence;
using Domain.Member.ValueObjects;
using Domain.Post.ValueObjects;
using MediatR;

namespace Application.Posts.Command.UpvotePost;

public class UpvotePostCommandHandler : IRequestHandler<UpvotePostCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpvotePostCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpvotePostCommand request, CancellationToken cancellationToken)
    {
        var post = _unitOfWork.PostRepository.GetById(PostId.Create(request.postId));
        if(post is null) throw new NoSuchPostException();

        if(post.UpvotingMemberIds.Contains(MemberId.Create(request.memberId))) { return; }

        _unitOfWork.PostRepository.UpvotePost(MemberId.Create(request.memberId), post);

        await _unitOfWork.SaveAsync();
        _unitOfWork.Dispose();

        return;
    }
}
