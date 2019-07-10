using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Opw.EntityFrameworkCore
{
    public static class DbContextConfigurer
    {
        private static readonly InMemoryDatabaseRoot _inMemoryDatabaseRoot = new InMemoryDatabaseRoot();

        public static void Configure(DbContextOptionsBuilder builder, string connectionString)
        {
            if (connectionString.IndexOf("inMemory", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                // don't raise the error warning us that the in memory db doesn't support transactions
                builder.ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));

                var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
                var dbName = connectionStringBuilder.InitialCatalog;

                builder.UseInMemoryDatabase(dbName, _inMemoryDatabaseRoot);
            }
            else
            {
                builder.UseSqlServer(connectionString);
            }
        }
    }
}
