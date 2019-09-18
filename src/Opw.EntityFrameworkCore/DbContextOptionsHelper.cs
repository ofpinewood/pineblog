using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data.SqlClient;

namespace Opw.EntityFrameworkCore
{
    public static class DbContextOptionsHelper
    {
        private static readonly InMemoryDatabaseRoot _inMemoryDatabaseRoot = new InMemoryDatabaseRoot();

        public static void ConfigureOptionsBuilder(DbContextOptionsBuilder builder, string connectionString)
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

        public static Action<DbContextOptionsBuilder> Configure(string connectionString)
        {
            return options =>
            {
                if (connectionString.IndexOf("inMemory", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    // don't raise the error warning us that the in memory db doesn't support transactions
                    options.ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));

                    var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
                    var dbName = connectionStringBuilder.InitialCatalog;

                    options.UseInMemoryDatabase(dbName, _inMemoryDatabaseRoot);
                }
                else
                {
                    options.UseSqlServer(connectionString);
                }
            };
            
        }
    }
}
