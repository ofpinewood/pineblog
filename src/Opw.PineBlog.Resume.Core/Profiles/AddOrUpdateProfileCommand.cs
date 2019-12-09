using MediatR;
using Opw.PineBlog.Resume.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Resume.Profiles
{
    /// <summary>
    /// Command that adds or updates a users' profile.
    /// </summary>
    public class AddOrUpdateProfileCommand : IRequest<Result<Profile>>
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string UserName { get; set; }
        public string Slug { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string Headline { get; set; }
        public string Industry { get; set; }
        public string Summary { get; set; }

        public string Country { get; set; }
        public string Region { get; set; }

        public ICollection<Link> Links { get; set; }
        public ICollection<Experience> Experiences { get; set; }
        public ICollection<Education> Education { get; set; }
        public ICollection<Skill> Skills { get; set; }
        public ICollection<Language> Languages { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Handler for the AddOrUpdateProfileCommand.
        /// </summary>
        public class Handler : IRequestHandler<AddOrUpdateProfileCommand, Result<Profile>>
        {
            private readonly IResumeEntityDbContext _context;

            /// <summary>
            /// Implementation of AddOrUpdateProfileCommand.Handler.
            /// </summary>
            /// <param name="context">The resume entity context.</param>
            public Handler(IResumeEntityDbContext context)
            {
                _context = context;
            }

            /// <summary>
            /// Handle the AddOrUpdateProfileCommand request.
            /// </summary>
            /// <param name="request">The AddOrUpdateProfileCommand request.</param>
            /// <param name="cancellationToken">A cancellation token.</param>
            public async Task<Result<Profile>> Handle(AddOrUpdateProfileCommand request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrWhiteSpace(request.Slug))
                    request.Slug = $"{request.FirstName} {request.LastName}".ToSlug();

                var entity = new Profile
                {
                    UserName = request.UserName,
                    Slug = request.Slug,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Headline = request.Headline,
                    Industry = request.Industry,
                    Summary = request.Summary,
                    Country = request.Country,
                    Region = request.Region,
                    Links = request.Links,
                    Experiences = request.Experiences,
                    Education = request.Education,
                    Skills = request.Skills,
                    Languages = request.Languages
                };

                _context.Profiles.Add(entity);
                var result = await _context.SaveChangesAsync(cancellationToken);
                if (!result.IsSuccess)
                    return Result<Profile>.Fail(result.Exception);

                return Result<Profile>.Success(entity);
            }
        }
    }
}
