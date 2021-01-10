using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Opw.PineBlog.MongoDb;
using Opw.PineBlog.RazorPages;

namespace Opw.PineBlog.Sample.NuGet
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

            if (Configuration.GetValue<string>("PineBlogDataSource") == "MongoDb")
            {
                // using MongoDb as the datasource
                services.AddPineBlogCore(Configuration);
                services.AddPineBlogMongoDb(Configuration);
            }
            else
            {
                // using EntityFrameworkCore, the default
                services.AddPineBlog(Configuration);
            }

            services.AddRazorPages()
                .AddPineBlogRazorPages()
                // the following extensions where added to validate/fix issue: https://github.com/ofpinewood/pineblog/issues/100 
                .AddRazorPagesOptions(o => o.Conventions.AuthorizeFolder("/somefolder"))
                .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
