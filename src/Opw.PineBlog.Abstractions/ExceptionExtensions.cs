using FluentValidation.Results;
using Opw.HttpExceptions;
using System.Linq;

namespace Opw.PineBlog
{
    //TODO: move to Opw.Common
    public static class ExceptionExtensions
    {
        public static string GetAggregatedExceptionMessage(this ValidationErrorException<ValidationFailure> ex)
        {
            return string.Join("", ex.Errors.Select(e => $"{e.Key}: {string.Join("", e.Value.Select(v => v.ErrorMessage))}"));
        }
    }
}
