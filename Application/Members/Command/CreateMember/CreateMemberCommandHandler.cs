using Application.Common.Exceptions;
using Application.Common.Interface.Persistence;
using Application.Common.Services;
using Application.Members.Common;
using Domain.CompanySpace.ValueObjects;
using Domain.Member;
using MediatR;

namespace Application.Members.Command.CreateMember;

public class CreateMemberCommandHandler : IRequestHandler<CreateMemberCommand, MemberResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtGenerator _jwtGenerator;

    public CreateMemberCommandHandler(IJwtGenerator jwtGenerator, IUnitOfWork unitOfWork)
    {
        _jwtGenerator = jwtGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<MemberResult> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        //Check if member already exists.
        var memberCheck = _unitOfWork.MemberRepository.GetMemberByEmail(request.Email);
        if (memberCheck is not null) throw new MemberAlreadyExistsException();

        //Get the company 
        var Space = _unitOfWork.SpaceRepository.GetSpaceById(CompanySpaceId.Create(request.CompanySpaceId));

        if (Space is null) throw new InvalidSpaceException();

        //Create a member

        var password = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var member = Member.Create(request.Name, request.Email, password, CompanySpaceId.Create(request.CompanySpaceId));

        //Save member
        Member mResult = _unitOfWork.MemberRepository.Add(member);
        _unitOfWork.SpaceRepository.AddMember(member, Space);

        await _unitOfWork.SaveAsync();
        _unitOfWork.Dispose();
        //Generate token
        string token = _jwtGenerator.GenerateJwt(mResult);

        //Create member Result
        MemberResult result = new(mResult.Name, mResult.Email, mResult.CompanySpaceId, Space.Name, token);

        return result;
    }
}
