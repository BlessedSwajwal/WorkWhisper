using Application.Common;
using Application.Common.Exceptions;
using Application.Common.Interface.Persistence;
using Application.Posts.Common;
using Domain.CompanySpace.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Posts.Query.GetPost;

public class PostQueryHandler : IRequestHandler<GetPostQuery, PostResponse>
{
    private readonly HttpContext _context;
    private readonly IPostRepository _postRepository;
    private readonly IMemberRepository _memberRepository;

    public PostQueryHandler(IHttpContextAccessor context, IPostRepository postRepository, IMemberRepository memberRepository, ISpaceRepository spaceRepository)
    {
        _context = context.HttpContext;
        _postRepository = postRepository;
        _memberRepository = memberRepository;
    }

    public async Task<PostResponse> Handle(GetPostQuery request, CancellationToken cancellationToken)
    {

        var member = _memberRepository.GetMemberById(MemberId.Create(_context.User.Claims.FirstOrDefault(cl => cl.Type == ClaimTypes.NameIdentifier)!.Value));

        
        var post = _postRepository.GetById(PostId.Create(request.PostId));
        var commentResults = new List<CommentResult>();

        if (post is null) throw new NoSuchPostException();

        if (!post.IsPrivate || member!.CompanySpaceId == post.SpaceId)
        {
            commentResults.AddRange(post.Comments.Select(comment =>
                new CommentResult(comment.Id.Value, comment.Text, comment.CommentorId.Value, comment.UpvotingMemberIds.Count)));

            return new PostResponse(post.Id.Value, post.Title, post.Body, post.SpaceId, post.IsPrivate, post.UpvotingMemberIds.Count,    commentResults);
        }
        else { 
            throw new UnauthorizedAccessException();
        }


     //   commentResults.AddRange(post.Comments.Select(comment =>
     //new CommentResult(comment.Id.Value, comment.Text, comment.CommentorId.Value, comment.UpvotingMemberIds.Count)));

     //   return new PostResponse(post.Id.Value, post.Title, post.Body, post.SpaceId, post.IsPrivate, commentResults);
    }
}
