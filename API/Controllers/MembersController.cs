using Application.Members.Command.CreateMember;
using Application.Members.Query.LoginMember;
using Contracts;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("members")]
[ApiController]
[AllowAnonymous]
public class MembersController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly IMapper _mapster;

    public MembersController(IMapper mapster, ISender mediator)
    {
        _mapster = mapster;
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterMemberRequest request)
    {
        var command =  _mapster.Map<CreateMemberCommand>(request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = _mapster.Map<LoginMemberQuery>(request);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
