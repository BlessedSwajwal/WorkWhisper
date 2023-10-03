using Application.Common.Exceptions;
using Application.Common.Interface.Persistence;
using Application.Common.Services;
using Application.Members.Common;
using Domain.Member;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Members.Query.LoginMember;

public class LoginMemberQueryHandler : IRequestHandler<LoginMemberQuery, MemberResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtGenerator _jwtGenerator;

    public LoginMemberQueryHandler(IJwtGenerator jwtGenerator, IUnitOfWork unitOfWork)
    {
        _jwtGenerator = jwtGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<MemberResult> Handle(LoginMemberQuery request, CancellationToken cancellationToken)
    {
        //Check if user exists.
        Member member = _unitOfWork.MemberRepository.GetMemberByEmail(request.Email);
        if (member is null) throw new InvalidCredentialsException();

        //Check password.
        var isCorrect = BCrypt.Net.BCrypt.Verify(request.Password, member.Password);

        if(!isCorrect) throw new InvalidCredentialsException();

        //Get the company space name
        var companyName = _unitOfWork.SpaceRepository.GetSpaceById(member.CompanySpaceId).Name;

        //Save changes and dispose UoW.
        await _unitOfWork.SaveAsync();
        _unitOfWork.Dispose();

        //Generate the token
        var token = _jwtGenerator.GenerateJwt(member);

        var memberResult = new MemberResult(member.Name, member.Email, member.CompanySpaceId, companyName, token);

        return memberResult;
    }
}
