using Application.Common.Exceptions;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IValidator<TRequest>? _validator;

    public ValidationBehavior(IValidator<TRequest>? validator)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if(_validator is null)
        {
            return await next();
        }

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if(validationResult.IsValid)
        {
            return await next();
        }

        //var result = validationResult.Errors.ConvertAll(er =>
        //{
        //    return new ValidationException($"{er.PropertyName}: {er.ErrorMessage}");
        //});

        var validationExceptions = new ValidationExceptions();
        foreach( var ex in validationResult.Errors )
        {
            validationExceptions.errors.Add($"{ex.PropertyName}: {ex.ErrorMessage}");
        }

        throw validationExceptions;
    }
}
