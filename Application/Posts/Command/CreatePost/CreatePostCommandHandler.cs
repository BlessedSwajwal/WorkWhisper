using Application.Common;
using Application.Common.Exceptions;
using Application.Common.Interface.Persistence;
using Application.Posts.Common;
using Domain.Member.ValueObjects;
using Domain.Post;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;


namespace Application.Posts.Command.CreatePost;

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, PostResponse>
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

    public async Task<PostResponse> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var memberId = MemberId.Create(Guid.Parse(_context.User.Claims.FirstOrDefault(cl => cl.Type == ClaimTypes.NameIdentifier).Value));

        var member = _memberRepository.GetMemberById(memberId);
        if (member is null) throw new MemberNotFoundException();

        Post post = Post.Create(
            title: request.Title,
            body: request.Body,
            spaceId: member.CompanySpaceId,
            memberId: memberId,
            isPrivate: request.IsPrivate);

        _postRepository.Add(post);


        //TODO - The following code should be handled using Domain Events. When saving the post, an event "PostCreated" should be fired. 
        #region EventRegion

        //Adding postId to the member
        member.AddPost(post.Id.Value);

        //Addind postId to the space
        var space = _spaceRepository.GetSpaceById(member.CompanySpaceId);
        if (space is null) throw new SpaceNotFoundException();
        space.AddPost(post.Id.Value);

        #endregion

        var postResult = new PostResponse(post.Id.Value, post.Title, post.Body, post.SpaceId.Value, post.IsPrivate, post.UpvotingMemberIds.Count, new List<CommentResult>());

        return postResult;
    }
}
