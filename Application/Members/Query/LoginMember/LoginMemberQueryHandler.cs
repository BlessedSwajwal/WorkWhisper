using Application.Common.Exceptions;
using Application.Common.Interface.Persistence;
using Application.Common.Services;
using Application.Members.Common;
using Domain.CompanySpace.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Members.Query.LoginMember;

public class LoginMemberQueryHandler : IRequestHandler<LoginMemberQuery, MemberResult>
{
    private readonly IMemberRepository _memberRepository;
    private readonly ISpaceRepository _spaceRepository;
    private readonly IJwtGenerator _jwtGenerator;

    public LoginMemberQueryHandler(IMemberRepository memberRepository, ISpaceRepository spaceRepository, IJwtGenerator jwtGenerator)
    {
        _memberRepository = memberRepository;
        _spaceRepository = spaceRepository;
        _jwtGenerator = jwtGenerator;
    }

    public async Task<MemberResult> Handle(LoginMemberQuery request, CancellationToken cancellationToken)
    {
        //Check if user exists.
        Member member = _memberRepository.GetMemberByEmail(request.Email);
        if (member is null) throw new InvalidCredentialsException();

        //Check password.
        var isCorrect = BCrypt.Net.BCrypt.Verify(request.Password, member.Password);

        if(!isCorrect) throw new InvalidCredentialsException();

        //Get the company space name
        var companyName = _spaceRepository.GetSpaceById(member.CompanySpaceId).Name;

        //Generate the token
        var token = _jwtGenerator.GenerateJwt(member);

        var memberResult = new MemberResult(member.Name, member.Email, member.CompanySpaceId, companyName, token);

        return memberResult;
    }
}
