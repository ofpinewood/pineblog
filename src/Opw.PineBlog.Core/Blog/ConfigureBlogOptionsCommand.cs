using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Opw.PineBlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Opw.PineBlog.Blog
{
    public class ConfigureBlogOptionsCommand : IRequest
    {
        public class Handler : IRequestHandler<ConfigureBlogOptionsCommand, Unit>
        {
            private readonly IOptions<BlogOptions> _blogOptions;
            private readonly IBlogEntityDbContext _context;

            public Handler(IBlogEntityDbContext context, IOptions<BlogOptions> blogOptions)
            {
                _blogOptions = blogOptions;
                _context = context;
            }

            public async Task<Unit> Handle(ConfigureBlogOptionsCommand request, CancellationToken cancellationToken)
            {
                var settings = _context.Settings
                    .Include(e => e.Cover)
                    .SingleOrDefault();

                if (settings == null)
                {
                    _context.Settings.Add(new Settings
                    {
                        Title = _blogOptions.Value.Title,
                        Description = _blogOptions.Value.Description,
                    });
                    await _context.SaveChangesAsync(cancellationToken);
                    return Unit.Value;
                }

                _blogOptions.Value.Title = settings.Title;
                _blogOptions.Value.Description = settings.Description;
                _blogOptions.Value.Cover = settings.Cover;

                return Unit.Value;
            }
        }
    }
}
