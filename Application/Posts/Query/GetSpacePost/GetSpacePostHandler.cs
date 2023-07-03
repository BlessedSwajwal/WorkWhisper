using Application.Common.Interface.Persistence;
using Application.Posts.Common;
using Domain.CompanySpace.Entity;
using Domain.CompanySpace.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Posts.Query.GetSpacePost;

public class GetSpacePostHandler : IRequestHandler<GetSpacePostQuery, List<PostResult>>
{
    private readonly ISpaceRepository _spaceRepository;
    private readonly IPostRepository _postRepository;
    private readonly HttpContext _context;

    public GetSpacePostHandler(ISpaceRepository spaceRepository, IHttpContextAccessor context, IPostRepository postRepository)
    {
        _spaceRepository = spaceRepository;
        _context = context.HttpContext;
        _postRepository = postRepository;
    }

    public async Task<List<PostResult>> Handle(GetSpacePostQuery request, CancellationToken cancellationToken)
    {
        //TODO - Use pagination

        bool memberOfSpace = false;
        List<PostResult> result = new List<PostResult>();
        var memberId = _context.User.Claims.FirstOrDefault(cl => cl.Type == ClaimTypes.NameIdentifier).Value;


        var spaceId = CompanySpaceId.Create(request.SpaceId);
        var space = _spaceRepository.GetSpaceById(spaceId);

        //Check if the member is of the space or not
        if (memberId is not null)
        {
            memberOfSpace = _spaceRepository.MemberExistsOrNot(spaceId, MemberId.Create(Guid.Parse(memberId)));
        }

        IReadOnlyCollection<PostId> postIds = _spaceRepository.GetAllPostId(spaceId);
        List<Post> posts = _postRepository.GetPostCollection(postIds);
        if (!memberOfSpace)
        {
            foreach (var post in posts)
            {
                result.Add(new PostResult(post.Title, post.Body, spaceId, post.IsPrivate));
            }
        }
        else
        {
            foreach (var post in posts)
            {
                if (post.IsPrivate) continue;
                result.Add(new PostResult(post.Title, post.Body, spaceId, post.IsPrivate));
            }
        }

        return result;
    }
}
