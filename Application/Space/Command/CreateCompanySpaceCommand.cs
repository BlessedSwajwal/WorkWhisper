using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Space.Command;

public record CreateCompanySpaceCommand (string Name) : IRequest<CompanySpaceResult>;

