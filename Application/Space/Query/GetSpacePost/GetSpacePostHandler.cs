using Application.Common;
using Application.Common.Interface.Persistence;
using Application.Posts.Common;
using Domain.CompanySpace.ValueObjects;
using Domain.Member.ValueObjects;
using Domain.Post;
using Domain.Post.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Space.Query.GetSpacePost;

public class GetSpacePostHandler : IRequestHandler<GetSpacePostQuery, List<PostResponse>>
{
    private readonly ISpaceRepository _spaceRepository;
    private readonly IPostRepository _postRepository;

    public GetSpacePostHandler(ISpaceRepository spaceRepository, IPostRepository postRepository)
    {
        _spaceRepository = spaceRepository;
        _postRepository = postRepository;
    }

    public async Task<List<PostResponse>> Handle(GetSpacePostQuery request, CancellationToken cancellationToken)
    {
        //TODO - Use pagination

        bool memberOfSpace = false;
        List<PostResponse> result = new();
        string? memberId = request.User.Claims.FirstOrDefault(cl => cl.Type == ClaimTypes.NameIdentifier)?.Value;


        var spaceId = CompanySpaceId.Create(request.SpaceId);
        var space = _spaceRepository.GetSpaceById(spaceId);

        //Check if the member is of the space or not
        if (memberId is not null)
        {
            memberOfSpace = _spaceRepository.MemberExistsOrNot(spaceId, MemberId.Create(Guid.Parse(memberId)));
        }

        IReadOnlyCollection<PostId> postIds = _spaceRepository.GetAllPostId(spaceId);
        List<Post> posts = _postRepository.GetPostCollection(postIds);
        var commentResults = new List<CommentResult>();

        foreach (var post in posts)
        {
            if (!memberOfSpace && post.IsPrivate){
                continue;
            }

            commentResults.AddRange(post.Comments.Select(comment =>
                  new CommentResult(comment.Id.Value, comment.Text,comment.CommentorId.Value, comment.UpvotingMemberIds.Count)));

            result.Add(new PostResponse(post.Id.Value, post.Title, post.Body, spaceId, post.IsPrivate,post.UpvotingMemberIds.Count, commentResults));
        }
        
        //else
        //{
        //    foreach (var post in posts)
        //    {
        //        if (post.IsPrivate) continue;

        //        var commentResults = new List<CommentResult>();
        //        commentResults.AddRange(post.Comments.Select(comment =>
        //            new CommentResult(comment.Id.Value, comment.Text, comment.CommentorId.Value, comment.UpvotingMemberIds.Count)));

        //        result.Add(new PostResponse(post.Id.Value, post.Title, post.Body, spaceId, post.IsPrivate, commentResults));
        //    }
        //}

        return result;
    }
}
