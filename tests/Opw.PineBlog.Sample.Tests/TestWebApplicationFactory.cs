using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Opw.PineBlog.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Opw.PineBlog.Sample
{
    public class TestWebApplicationFactory : WebApplicationFactory<Startup>
    {
        public IConfigurationRoot Configuration { get; }
        private SetCookieHeaderValue _authenticationCookie;
        private SetCookieHeaderValue _antiforgeryCookie;
        private string _antiforgeryToken;
        private static Regex _antiforgeryFormFieldRegex = new Regex(@"\<input name=""__RequestVerificationToken"" type=""hidden"" value=""([^""]+)"" \/\>");

        public TestWebApplicationFactory()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return new WebHostBuilder()
                .UseConfiguration(Configuration)
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((_, config) => config.AddPineBlogConfiguration(reloadOnChange: true));
        }

        public async Task<string> EnsureAntiforgeryToken(HttpClient client)
        {
            SetCookieHeaderValue antiforgeryCookie = null;
            var response = await client.GetAsync("/Account/Login");
            response.EnsureSuccessStatusCode();
            if (response.Headers.TryGetValues("Set-Cookie", out var values))
                antiforgeryCookie = SetCookieHeaderValue.ParseList(values.ToList())
                    .SingleOrDefault(c => c.Name.StartsWith(".AspNetCore.AntiForgery.", StringComparison.InvariantCultureIgnoreCase));

            antiforgeryCookie.Should().NotBeNull();

            client.DefaultRequestHeaders.Add("Cookie", new CookieHeaderValue(antiforgeryCookie.Name, antiforgeryCookie.Value).ToString());

            var responseHtml = await response.Content.ReadAsStringAsync();

            var antiforgeryFormFieldRegex = new Regex(@"\<input name=""__RequestVerificationToken"" type=""hidden"" value=""([^""]+)"" \/\>");
            var match = antiforgeryFormFieldRegex.Match(responseHtml);
            var antiforgeryToken = match.Success ? match.Groups[1].Captures[0].Value : null;

            antiforgeryToken.Should().NotBeNull();

            return antiforgeryToken;
        }

        protected async Task<Dictionary<string, string>> EnsureAntiforgeryTokenForm(HttpClient client, Dictionary<string, string> formData = null)
        {
            if (formData == null) formData = new Dictionary<string, string>();
            formData.Add("__RequestVerificationToken", await EnsureAntiforgeryToken(client));
            return formData;
        }

        public async Task EnsureAuthenticationCookie(HttpClient client)
        {
            if (_authenticationCookie != null) return;

            var formData = await EnsureAntiforgeryTokenForm(client, new Dictionary<string, string>
            {
                { "Input.Email", "pineblog@example.com" },
                { "Input.Password", "demo" }
            });
            var response = await client.PostAsync("/Account/Login", new FormUrlEncodedContent(formData));

            response.StatusCode.Should().Be(HttpStatusCode.Redirect);

            if (response.Headers.TryGetValues("Set-Cookie", out var values))
            {
                _authenticationCookie = SetCookieHeaderValue.ParseList(values.ToList()).SingleOrDefault(c => c.Name.StartsWith("AUTHENTICATION_COOKIE", StringComparison.InvariantCultureIgnoreCase));
            }

            _authenticationCookie.Should().NotBeNull();

            client.DefaultRequestHeaders.Add("Cookie", new CookieHeaderValue(_authenticationCookie.Name, _authenticationCookie.Value).ToString());

            // The current pair of antiforgery cookie-token is not valid anymore
            // Since the tokens are generated based on the authenticated user!
            // We need a new token after authentication (The cookie can stay the same)
            _antiforgeryToken = null;
        }
    }
}
