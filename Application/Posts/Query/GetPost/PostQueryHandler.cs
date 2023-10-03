using Application.Common;
using Application.Common.Exceptions;
using Application.Common.Interface.Persistence;
using Application.Posts.Common;
using Domain.Member.ValueObjects;
using Domain.Post.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Posts.Query.GetPost;

public class PostQueryHandler : IRequestHandler<GetPostQuery, PostResponse>
{
    private readonly HttpContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public PostQueryHandler(IHttpContextAccessor context, IUnitOfWork unitOfWork)
    {
        _context = context.HttpContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<PostResponse> Handle(GetPostQuery request, CancellationToken cancellationToken)
    {

        var member = _unitOfWork.MemberRepository.GetMemberById(MemberId.Create(_context.User.Claims.FirstOrDefault(cl => cl.Type == ClaimTypes.NameIdentifier)!.Value));

        
        var post = _unitOfWork.PostRepository.GetById(PostId.Create(request.PostId));

        await _unitOfWork.SaveAsync();
        _unitOfWork.Dispose();

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
    }
}
