using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Opw.PineBlog.Sample;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Opw.PineBlog.EntityFrameworkCore;
using Opw.PineBlog.MongoDb;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Opw.PineBlog.RazorPages;
using Opw.PineBlog.Sample.Middleware;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Opw.PineBlog;

var builder = WebApplication.CreateBuilder(args);

if (builder.Configuration.UseInMemoryMongoDb())
{
    builder.Configuration.AddInMemoryMongoDbConfiguration();
    // using MongoDb as the datasource
    builder.Configuration.AddPineBlogMongoDbConfiguration(reloadOnChange: true);
}
else
{
    // using EntityFrameworkCore, the default
    builder.Configuration.AddPineBlogEntityFrameworkCoreConfiguration(reloadOnChange: true);
}

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddApplicationInsights();
builder.Logging.AddConsole();


builder.Services.AddApplicationInsightsTelemetry();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

if (builder.Configuration.GetValue<string>("PineBlogDataSource") == "MongoDb")
{
    // using MongoDb as the datasource
    builder.Services.AddPineBlogCore(builder.Configuration);
    builder.Services.AddPineBlogMongoDb(builder.Configuration);
}
else
{
    // using EntityFrameworkCore, the default
    builder.Services.AddPineBlog(builder.Configuration);
}

builder.Services.AddRazorPages()
    .AddPineBlogRazorPages();

builder.Services.AddRouting(options => {
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
    options.AppendTrailingSlash = true;
});

var app = builder.Build();

app.InitializeSampleDb();

app.UseMiddleware<StopApplicationMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();

#region Sample Extensions

public static class WebApplicationExtensions
{
    /// <summary>
    /// Initialize the sample db.
    /// </summary>
    /// <remarks>Do not use in production!</remarks>
    public static WebApplication InitializeSampleDb(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;
            var config = serviceProvider.GetRequiredService<IConfiguration>();

            try
            {
                if (config.GetValue<string>("PineBlogDataSource") == "MongoDb")
                {
                    // using MongoDb
                    serviceProvider.InitializePineBlogMongoDb((database) => new MongoDbSeed(database).Run());
                }
                else
                {
                    // using EntityFrameworkCore
                    serviceProvider.InitializePineBlogDatabase((context) => new DatabaseSeed(context).Run());
                }
            }
            catch (Exception ex)
            {
                var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while migrating the database.");
            }
        }

        return app;
    }
}

public static class ConfigurationBuilderExtensions
{
    /// <summary>
    /// Check if we need to initialize the MongoDb sample database.
    /// </summary>
    /// <remarks>Do not use in production!</remarks>
    public static bool UseInMemoryMongoDb(this IConfigurationBuilder builder)
    {
        var configuration = builder.Build();
        return configuration.GetValue<string>("PineBlogDataSource") == "MongoDb";
    }

    /// <summary>
    /// Add the in-memory configuration for the MongoDb sample database.
    /// </summary>
    /// <remarks>Do not use in production!</remarks>
    public static IConfigurationBuilder AddInMemoryMongoDbConfiguration(this IConfigurationBuilder builder)
    {
        var configuration = builder.Build();
        if (configuration.GetValue<string>("PineBlogDataSource") == "MongoDb")
        {
            // override the ConnectionStringName appsetting to point to the MongoDbConnection
            var settings = new Dictionary<string, string> { { "PineBlogOptions:ConnectionStringName", "MongoDbConnection" } };
            builder.AddInMemoryCollection(settings);

            // initializes an in-memory MongoDbRunner when needed
            MongoDbInMemoryRunner.Instance.Initialize(builder);
        }

        return builder;
    }
}

#endregion