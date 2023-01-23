using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freezone.Core.CrossCuttingConcerns.Exceptions.Handlers;

public abstract class ExceptionHandler
{
    public Task HandleExceptionAsync(Exception exception) 
    {
        if (exception is BusinessException businessException) 
            return HandleException(businessException);

        if (exception is ValidationException validationException)
            return HandleException(validationException);

        return HandleException(exception);
    }

    protected abstract Task HandleException(BusinessException exception);
    protected abstract Task HandleException(ValidationException exception);
    protected abstract Task HandleException(Exception exception);
}
