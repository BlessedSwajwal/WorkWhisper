using Application.Common.Interface.Persistence;
using Domain.CompanySpace;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Space.Command;

internal class CreateSpaceCommandHandler : IRequestHandler<CreateCompanySpaceCommand, CompanySpaceResult>
{
    private readonly ISpaceRepository _spaceRepository;

    public CreateSpaceCommandHandler(ISpaceRepository spaceRepository)
    {
        _spaceRepository = spaceRepository;
    }

    public async Task<CompanySpaceResult> Handle(CreateCompanySpaceCommand request, CancellationToken cancellationToken)
    {
        var space = CompanySpace.Create(request.Name);

        _spaceRepository.AddSpace(space);
        var spaceResult = new CompanySpaceResult(space.Name);

        return spaceResult;
    }
}
