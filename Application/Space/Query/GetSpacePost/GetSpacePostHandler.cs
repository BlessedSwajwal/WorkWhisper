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
    private readonly IUnitOfWork _unitOfWork;

    public GetSpacePostHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<PostResponse>> Handle(GetSpacePostQuery request, CancellationToken cancellationToken)
    {
        //TODO - Use pagination. Since the posts number are faily small, I am not using.

        bool memberOfSpace = false;
        List<PostResponse> result = new();
        string? memberId = request.User.Claims.FirstOrDefault(cl => cl.Type == ClaimTypes.NameIdentifier)?.Value;


        var spaceId = CompanySpaceId.Create(request.SpaceId);
        var space = _unitOfWork.SpaceRepository.GetSpaceById(spaceId);

        //Check if the member is of the space or not
        if (memberId is not null)
        {
            memberOfSpace = _unitOfWork.SpaceRepository.MemberExistsOrNot(spaceId, MemberId.Create(Guid.Parse(memberId)));
        }

        //Retrieve all the posts from the DB.
        IReadOnlyCollection<PostId> postIds = _unitOfWork.SpaceRepository.GetAllPostId(spaceId);
        List<Post> posts = _unitOfWork.PostRepository.GetPostCollection(postIds);

        await _unitOfWork.SaveAsync();
        _unitOfWork.Dispose();
        
        //Adding comment result to the response.
        foreach (var post in posts)
        {
            var commentResults = new List<CommentResult>();
            if (!memberOfSpace && post.IsPrivate)
            {
                continue;
            }
            commentResults.AddRange(post.Comments.Select(comment =>
                  new CommentResult(comment.Id.Value, comment.Text,comment.CommentorId.Value, comment.UpvotingMemberIds.Count)));

            result.Add(new PostResponse(post.Id.Value, post.Title, post.Body, spaceId, post.IsPrivate,post.UpvotingMemberIds.Count, commentResults));
        }

        return result;
    }
}
