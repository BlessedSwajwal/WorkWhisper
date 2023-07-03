using Application.Posts.Command;
using Application.Posts.Query.GetPost;
using Contracts;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
}
