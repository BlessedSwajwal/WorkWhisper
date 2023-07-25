using Application.Comments.Command.CreateComment;
using Application.Comments.Command.UpvoteComment;
using Application.Posts.Command.CreatePost;
using Application.Posts.Command.UpvotePost;
using Application.Posts.Query.GetPost;
using Contracts;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

[Route("posts")]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly IMapper _mapster;
    private readonly ISender _mediator;

    public PostsController(IMapper mapster, ISender mediator)
    {
        _mapster = mapster;
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreatePost(CreatePostRequest request)
    {
        var command = _mapster.Map<CreatePostCommand>(request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{postId}")]
    public async Task<IActionResult> GetPost([FromRoute] Guid postId)
    {
        var query = new GetPostQuery(postId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("{postId}/comment")]
    public async Task<IActionResult> PostComment([FromRoute] Guid postId, CommentRequest request)
    {
        request.PostId = postId;
        var command = _mapster.Map<CreateCommentCommand>(request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("{postId}/{commentId}/upvote")]
    public async Task<IActionResult> UpvoteComment([FromRoute] Guid postId, [FromRoute] Guid commentId)
    {
        var command = new UpvoteCommentCommand(Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value), postId, commentId);

        await _mediator.Send(command);
        return Ok();
    }

    [HttpPost("{postId}/upvote")]
    public async Task<IActionResult> UpvotePost([FromRoute] Guid postId)
    {
        var command = new UpvotePostCommand(Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), postId);

        await _mediator.Send(command);
        return Ok();
    }
}
