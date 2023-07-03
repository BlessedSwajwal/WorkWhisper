using Application.Space.Command;
using Contracts;
using MapsterMapper;
using MediatR;
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

    [HttpPost("Create")]
    public async Task<IActionResult> CreateSpace(CreateSpaceRequest request)
    {
        var command = _mapster.Map<CreateCompanySpaceCommand>(request);
        await _mediator.Send(command);
        return Ok();
    }
}
