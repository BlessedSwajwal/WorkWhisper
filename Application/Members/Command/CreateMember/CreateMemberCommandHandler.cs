using Application.Common.Exceptions;
using Application.Common.Interface.Persistence;
using Application.Common.Services;
using Application.Members.Common;
using Domain.CompanySpace.Entity;
using Domain.CompanySpace.ValueObjects;
using MediatR;

namespace Application.Members.Command.CreateMember;

public class CreateMemberCommandHandler : IRequestHandler<CreateMemberCommand, MemberResult>
{
    private readonly IMemberRepository _memberRepository;
    private readonly ISpaceRepository _spaceRepository;
    private readonly IJwtGenerator _jwtGenerator;

    public CreateMemberCommandHandler(IMemberRepository memberRepository, ISpaceRepository spaceRepository, IJwtGenerator jwtGenerator)
    {
        _memberRepository = memberRepository;
        _spaceRepository = spaceRepository;
        _jwtGenerator = jwtGenerator;
    }

    public async Task<MemberResult> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        //Check if member already exists.
        var memberCheck = _memberRepository.GetMemberByEmail(request.Email);
        if (memberCheck is not null) throw new MemberAlreadyExistsException();

        //Create a member

        var password = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var member = Member.Create(request.Name, request.Email, password, CompanySpaceId.Create(request.CompanySpaceId));

        //Save member
        Member mResult = _memberRepository.Add(member);

        //Get the company Name
        var Space = _spaceRepository.GetSpaceById(mResult.CompanySpaceId);

        string Name = (Space is null) ? "TempUser" : Space.Name;

        //Generate token
        string token = _jwtGenerator.GenerateJwt(mResult);

        //Create member Result
        MemberResult result = new(mResult.Name, mResult.Email, mResult.CompanySpaceId, Name, token);

        return result;
    }
}
