using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Exceptions;

public class ValidationExceptions : Exception
{
    public int Code = (int)HttpStatusCode.BadRequest;

    public List<string> errors { get; set; } = new();

    public override string ToString()
    {
        string result = "";
        foreach (var error in errors)
        {
            result += $"{error}\n";
        }
        return result;
    }
}
