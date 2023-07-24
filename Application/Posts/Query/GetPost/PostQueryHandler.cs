using Application.Common.Exceptions;
using Application.Common.Interface.Persistence;
using Application.Posts.Common;
using Domain.CompanySpace.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Posts.Query.GetPost;

public class PostQueryHandler : IRequestHandler<GetPostQuery, PostResult>
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

    public async Task<PostResult> Handle(GetPostQuery request, CancellationToken cancellationToken)
    {

        var member = _memberRepository.GetMemberById(MemberId.Create(_context.User.Claims.FirstOrDefault(cl => cl.Type == ClaimTypes.NameIdentifier)!.Value));

        
        var post = _postRepository.GetById(PostId.Create(request.PostId));
        if (post is null) throw new NoSuchPostException();

        if (member is not null && member.CompanySpaceId == post.SpaceId)
        {
            return new PostResult(post.Id.Value, post.Title, post.Body, post.SpaceId, post.IsPrivate, post.Comments);
        }
        else if (post.IsPrivate) { 
            throw new UnauthorizedAccessException();
        }

        return new PostResult(post.Id.Value, post.Title, post.Body, post.SpaceId, post.IsPrivate, post.Comments);
    }
}
