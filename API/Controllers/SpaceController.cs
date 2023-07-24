using Application.Space.Command;
using Application.Space.Query.GetAllSpaces;
using Application.Space.Query.GetSpacePost;
using Contracts;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("Space")]
[ApiController]
public class SpaceController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly IMapper _mapster;
    public SpaceController(ISender mediator, IMapper mapster)
    {
        _mediator = mediator;
        _mapster = mapster;
    }

    [AllowAnonymous]
    [HttpPost("Create")]
    public async Task<IActionResult> CreateSpace(CreateSpaceRequest request)
    {
        var command = _mapster.Map<CreateCompanySpaceCommand>(request);
        await _mediator.Send(command);
        return Ok();
    }

  //  [AllowAnonymous]
    [HttpGet("{spaceId}")]
    public async Task<IActionResult> GetSpacePosts([FromRoute] Guid spaceId)
    {
        var query = new GetSpacePostQuery(spaceId, User);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("AllSpaces")]
    public async Task<IActionResult> GetAllSpace()
    {
        var query = new GetAllSpacesQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
