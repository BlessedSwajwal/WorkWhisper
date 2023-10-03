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
    private readonly IUnitOfWork _unitOfWork;

    public CreateSpaceCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CompanySpaceResult> Handle(CreateCompanySpaceCommand request, CancellationToken cancellationToken)
    {
        var space = CompanySpace.Create(request.Name);

        _unitOfWork.SpaceRepository.AddSpace(space);
        await _unitOfWork.SaveAsync();
        _unitOfWork.Dispose();

        var spaceResult = new CompanySpaceResult(space.Name);
        return spaceResult;
    }
}
