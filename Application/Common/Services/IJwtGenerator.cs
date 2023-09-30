using Domain.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Services;

public interface IJwtGenerator
{
    string GenerateJwt(Member member);
}
