using Opw.PineBlog.Resume.Entities;
using Opw.PineBlog.Resume.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Text;
using WaffleGenerator;

namespace Opw.PineBlog.Sample
{
    internal class PineBlogResumeDatabaseSeed
    {
        private readonly ResumeEntityDbContext _dbContext;

        public PineBlogResumeDatabaseSeed(ResumeEntityDbContext context)
        {
            _dbContext = context;
        }

        public void Run()
        {
            CreateProfile();
        }

        void CreateProfile()
        {
            if (_dbContext.Profiles.Count() > 0) return;

            var email = ApplicationConstants.UserEmail;
            if (_dbContext.Profiles.Count(a => a.UserName.Equals(email)) > 0) return;

            _dbContext.Profiles.Add(new Profile
            {
                UserName = email,
                Email = email,
                Headline = WaffleEngine.Title(),
                Summary = WaffleEngine.Text(1, false),
            });

            _dbContext.SaveChanges();
        }
    }
}
