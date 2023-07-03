using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Exceptions;

public class MemberAlreadyExistsException : Exception, IServiceException
{
    public int StatusCode => (int)HttpStatusCode.BadRequest;

    public string? ErrorMessage => "The email address is already associated with an account.";
}
