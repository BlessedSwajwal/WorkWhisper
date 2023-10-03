using Application.Common.Interface.Persistence;
using Domain.Member.ValueObjects;
using Domain.Post.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Comments.Command.UpvoteComment;

public class UpvoteCommentCommandHandler : IRequestHandler<UpvoteCommentCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpvoteCommentCommandHandler( IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpvoteCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = _unitOfWork.PostRepository.GetComment(PostId.Create(request.PostId), CommentId.Create(request.CommentId));

        if (comment.UpvotingMemberIds.Contains(MemberId.Create(request.UserId))) { return; }

        comment.Upvote(MemberId.Create(request.UserId));
        await _unitOfWork.SaveAsync();
        _unitOfWork.Dispose();
    }
}
