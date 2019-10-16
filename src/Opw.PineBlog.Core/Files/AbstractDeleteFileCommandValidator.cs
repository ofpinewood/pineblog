using FluentValidation;
using Opw.FluentValidation;
using System.IO;

namespace Opw.PineBlog.Files
{
    /// <summary>
    /// Base class for validators for DeleteFileCommand requests.
    /// </summary>
    public abstract class AbstractDeleteFileCommandValidator<TRequest> : AbstractValidator<TRequest>
        where TRequest : IDeleteFileCommand
    {
        /// <summary>
        /// Implementation of AbstractUploadFileCommandValidator.
        /// </summary>
        public AbstractDeleteFileCommandValidator()
        {
            RuleFor(c => c.FileName).NotEmpty();
        }
    }
}
