using Application.Common.Interface.Persistence;
using Domain.CompanySpace.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Comments.Command.UpvoteComment;

public class UpvoteCommentCommandHandler : IRequestHandler<UpvoteCommentCommand>
{
    private readonly IPostRepository _postRepository;

    public UpvoteCommentCommandHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task Handle(UpvoteCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = _postRepository.GetComment(PostId.Create(request.PostId), CommentId.Create(request.CommentId));

        //TO DO - Check if the user has already upvoted the comment.
        comment.Upvote(MemberId.Create(request.UserId));
    }
}
