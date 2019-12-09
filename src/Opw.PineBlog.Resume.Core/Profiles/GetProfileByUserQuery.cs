using MediatR;
using Microsoft.EntityFrameworkCore;
using Opw.HttpExceptions;
using Opw.PineBlog.Resume.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Resume.Profiles
{
    public class GetProfileByUserQuery : IRequest<Result<Profile>>
    {
        public string UserName { get; set; }

        public class Handler : IRequestHandler<GetProfileByUserQuery, Result<Profile>>
        {
            private readonly IResumeEntityDbContext _context;

            public Handler(IResumeEntityDbContext context)
            {
                _context = context;
            }

            public async Task<Result<Profile>> Handle(GetProfileByUserQuery request, CancellationToken cancellationToken)
            {
                var profile = await _context.Profiles
                    .Include(p => p.Education)
                    .Include(p => p.Experiences)
                    .Include(p => p.Languages)
                    .Include(p => p.Links)
                    .Include(p => p.Skills)
                    .Where(p => p.UserName.Equals(request.UserName))
                    .SingleOrDefaultAsync(cancellationToken);

                if (profile == null)
                    return Result<Profile>.Fail(new NotFoundException<Profile>($"Could not find profile for user: \"{request.UserName}\""));

                return Result<Profile>.Success(profile);
            }
        }
    }
}
