using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Members.Common;

public record MemberResult
(
    string Name,
    string Email,
    Guid CompanySpaceId,
    string CompanySpaceName,
    string token
);
