using Application.Common.Exceptions;
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

namespace Application.Posts.Command;

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, PostResult>
{
    private readonly HttpContext _context;
    private readonly ISpaceRepository _spaceRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IPostRepository _postRepository;

    public CreatePostCommandHandler(IHttpContextAccessor context, IMemberRepository memberRepository, ISpaceRepository spaceRepository, IPostRepository postRepository)
    {
        _context = context.HttpContext;
        _memberRepository = memberRepository;
        _spaceRepository = spaceRepository;
        _postRepository = postRepository;
    }

    public async Task<PostResult> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var memberId = MemberId.Create(Guid.Parse(_context.User.Claims.FirstOrDefault(cl => cl.Type == ClaimTypes.NameIdentifier).Value));

        var member = _memberRepository.GetMemberById(memberId);


        Post post = Post.Create(
            title: request.title,
            body: request.body,
            spaceId: member.CompanySpaceId,
            memberId: memberId,
            isPrivate: request.isPrivate);

        _postRepository.Add(post);


        //TODO - The following code should be handled using Domain Events. When saving the post, an event "PostCreated" should be fired. 
        #region EventRegion

        //Adding postId to the member
        member.AddPost(post.Id);

        //Addind postId to the space
        var space = _spaceRepository.GetSpaceById(member.CompanySpaceId);
        if (space is null) throw new SpaceNotFoundException();
        space.AddPost(post.Id);

        #endregion

        var postResult = new PostResult(post.Title, post.Body, post.SpaceId.Value, post.IsPrivate);

        return postResult;
    }
}
