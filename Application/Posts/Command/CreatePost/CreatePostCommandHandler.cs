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
    private readonly IUnitOfWork _unitOfWork;

    public CreatePostCommandHandler(IHttpContextAccessor context, IUnitOfWork unitOfWork)
    {
        _context = context.HttpContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<PostResponse> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var memberId = MemberId.Create(Guid.Parse(_context.User.Claims.FirstOrDefault(cl => cl.Type == ClaimTypes.NameIdentifier).Value));

        var member = _unitOfWork.MemberRepository.GetMemberById(memberId);
        if (member is null) throw new MemberNotFoundException();

        Post post = Post.Create(
            title: request.Title,
            body: request.Body,
            spaceId: member.CompanySpaceId,
            memberId: memberId,
            isPrivate: request.IsPrivate);

        _unitOfWork.PostRepository.Add(post);



        //Adding postId to the member
        member.AddPost(post.Id.Value);

        _unitOfWork.MemberRepository.UpdateMember(member);

        //Addind postId to the space
        var space = _unitOfWork.SpaceRepository.GetSpaceById(member.CompanySpaceId);
        if (space is null) throw new SpaceNotFoundException();
        space.AddPost(post.Id.Value);

        _unitOfWork.SpaceRepository.UpdateSpace(space);

        await _unitOfWork.SaveAsync();
        _unitOfWork.Dispose();

        var postResult = new PostResponse(post.Id.Value, post.Title, post.Body, post.SpaceId.Value, post.IsPrivate, post.UpvotingMemberIds.Count, new List<CommentResult>());

        return postResult;
    }
}
