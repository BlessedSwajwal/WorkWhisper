using Application.Common.Exceptions;
using Application.Common.Interface.Persistence;
using Domain.CompanySpace.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Posts.Command.UpvotePost;

public class UpvotePostCommandHandler : IRequestHandler<UpvotePostCommand>
{
    private readonly IPostRepository _postRepository;

    public UpvotePostCommandHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public Task Handle(UpvotePostCommand request, CancellationToken cancellationToken)
    {
        var post = _postRepository.GetById(PostId.Create(request.postId));
        if(post is null) throw new NoSuchPostException();

        if(post.UpvotingMemberIds.Contains(MemberId.Create(request.memberId))) { return Task.CompletedTask; }

        _postRepository.UpvotePost(MemberId.Create(request.memberId), post);

        return Task.CompletedTask;
    }
}
