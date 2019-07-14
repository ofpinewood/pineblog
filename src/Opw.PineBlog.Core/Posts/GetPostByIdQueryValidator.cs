﻿using FluentValidation;
using Opw.FluentValidation;

namespace Opw.PineBlog.Posts
{

    /// <summary>
    /// Validator GetPostByIdQuery request.
    /// </summary>
    public class GetPostByIdQueryValidator : AbstractValidator<GetPostByIdQuery>
    {
        /// <summary>
        /// Implementation of GetPostQueryValidator.
        /// </summary>
        public GetPostByIdQueryValidator()
        {
            RuleFor(c => c.Id).IsRequiredGuid();
        }
    }
}
