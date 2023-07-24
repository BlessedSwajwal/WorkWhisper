using Application.Common.Interface.Persistence;
using Application.Posts.Common;
using Domain.CompanySpace.Entity;
using Domain.CompanySpace.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Space.Query.GetSpacePost;

public class GetSpacePostHandler : IRequestHandler<GetSpacePostQuery, List<PostResult>>
{
    private readonly ISpaceRepository _spaceRepository;
    private readonly IPostRepository _postRepository;

    public GetSpacePostHandler(ISpaceRepository spaceRepository, IPostRepository postRepository)
    {
        _spaceRepository = spaceRepository;
        _postRepository = postRepository;
    }

    public async Task<List<PostResult>> Handle(GetSpacePostQuery request, CancellationToken cancellationToken)
    {
        //TODO - Use pagination

        bool memberOfSpace = false;
        List<PostResult> result = new List<PostResult>();
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
        if (memberOfSpace)
        {
            foreach (var post in posts)
            {
                result.Add(new PostResult(post.Id.Value, post.Title, post.Body, spaceId, post.IsPrivate, post.Comments));
            }
        }
        else
        {
            foreach (var post in posts)
            {
                if (post.IsPrivate) continue;
                result.Add(new PostResult(post.Id.Value, post.Title, post.Body, spaceId, post.IsPrivate, post.Comments));
            }
        }

        return result;
    }
}
