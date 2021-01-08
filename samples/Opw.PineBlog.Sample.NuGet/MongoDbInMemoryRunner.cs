using Microsoft.Extensions.Configuration;
using Mongo2Go;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Opw.PineBlog.Sample.NuGet
{
    internal class MongoDbInMemoryRunner : IDisposable
    {
        private MongoDbRunner _runner;

        /// <summary>
        /// Initializes an in-memory MongoDbRunner when needed.
        /// </summary>
        public void Initialize(IConfigurationBuilder config)
        {
            var configuration = config.Build();
            var blogOptions = configuration.GetSection(nameof(PineBlogOptions)).Get<PineBlogOptions>();
            var connectionString = configuration.GetConnectionString(blogOptions.ConnectionStringName);

            if (connectionString.IndexOf("inMemory", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                _runner = MongoDbRunner.Start(singleNodeReplSet: true);

                // override the ConnectionStrings appsetting to use the connection string from the runner
                var settings = new Dictionary<string, string> { { $"ConnectionStrings:{blogOptions.ConnectionStringName}", _runner.ConnectionString } };
                config.AddInMemoryCollection(settings);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }

            // always try to dispose, even if disposing=false
            if (_runner != null)
            {
                // wait a little to prevent timeouts
                Thread.Sleep(500);
                try { _runner?.Dispose(); } catch { }
                _runner = null;
                GC.Collect();
            }
        }
    }
}
