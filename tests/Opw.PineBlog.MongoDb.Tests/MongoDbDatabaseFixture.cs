using Mongo2Go;
using System;
using System.Threading;
using Xunit;

namespace Opw.PineBlog.MongoDb
{
    public sealed class MongoDbDatabaseFixture : IDisposable
    {
        public MongoDbRunner Runner { get; private set; }

        public MongoDbDatabaseFixture()
        {
            Runner = MongoDbRunner.Start(singleNodeReplSet: true);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing) { }

            // always try to dispose, even if disposing=false
            if (Runner != null)
            {
                // wait a little to prevent timeouts
                Thread.Sleep(500);
                try { Runner?.Dispose(); } catch { }
                Runner = null;
                GC.Collect();
            }
        }
    }
}
