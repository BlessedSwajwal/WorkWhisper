using Application.Common.Interface.Persistence;
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

namespace Application.Comments.Command.CreateComment;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand>
{
    private readonly HttpContext _context;
    private readonly IPostRepository _postRepository;

    public CreateCommentCommandHandler(IHttpContextAccessor context, IPostRepository commentRepository)
    {
        _context = context.HttpContext;
        _postRepository = commentRepository;
    }

    public async Task Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        //Get the member who commented.
        var memberId = MemberId.Create(Guid.Parse(_context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value));

        if (memberId is null) throw new Exception("Error creating memberId");

        //Create the comment.
        var comment = Comment.Create(request.Comment, memberId);

        //save comment to post
        _postRepository.AddComment(request.PostId, comment);

        return;
    }
}
