﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Exceptions;

public class InvalidCredentialsException : Exception, IServiceException
{
    public int StatusCode => (int)HttpStatusCode.NotFound;

    public string? ErrorMessage => "Invalid credentials.";
}
