using Application.Common.Interface.Persistence;
using Domain.CompanySpace;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Space.Query.GetAllSpaces;

public class GetAllSpaceQueryHandler : IRequestHandler<GetAllSpacesQuery, List<CompanySpaceResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllSpaceQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<CompanySpaceResult>> Handle(GetAllSpacesQuery request, CancellationToken cancellationToken)
    {
        List<CompanySpaceResult> cs = _unitOfWork.SpaceRepository.GetAllSpaces();
        _unitOfWork.Dispose();
        return cs;
    }
}
