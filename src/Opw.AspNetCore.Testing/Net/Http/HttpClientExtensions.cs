using FluentAssertions;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Opw.AspNetCore.Testing.Net.Http
{
    public static class HttpClientExtensions
    {
        private static readonly Regex _antiforgeryFormFieldRegex = new Regex(@"\<input name=""__RequestVerificationToken"" type=""hidden"" value=""([^""]+)"" \/\>");

        public static async Task<string> GetAntiforgeryTokenAsync(this HttpClient client, AuthenticationContext authenticationContext)
        {
            SetCookieHeaderValue antiforgeryCookie = null;

            var response = await client.GetAsync(authenticationContext.LoginUri);
            response.EnsureSuccessStatusCode();

            if (response.Headers.TryGetValues("Set-Cookie", out var values))
            {
                antiforgeryCookie = SetCookieHeaderValue.ParseList(values.ToList())
                    .SingleOrDefault(c => c.Name.StartsWith(".AspNetCore.AntiForgery.", StringComparison.InvariantCultureIgnoreCase));
            }

            antiforgeryCookie.Should().NotBeNull();

            client.DefaultRequestHeaders.Add("Cookie", new CookieHeaderValue(antiforgeryCookie.Name, antiforgeryCookie.Value).ToString());

            var responseHtml = await response.Content.ReadAsStringAsync();

            var match = _antiforgeryFormFieldRegex.Match(responseHtml);
            var antiforgeryToken = match.Success ? match.Groups[1].Captures[0].Value : null;

            antiforgeryToken.Should().NotBeNull();

            return antiforgeryToken;
        }

        public static async Task<Dictionary<string, string>> GetAntiforgeryTokenFormAsync(this HttpClient client, AuthenticationContext authenticationContext)
        {
            var formData = new Dictionary<string, string>();
            formData.Add("__RequestVerificationToken", await client.GetAntiforgeryTokenAsync(authenticationContext));
            return formData;
        }

        public static async Task EnsureAuthenticationCookieAsync(this HttpClient client, AuthenticationContext authenticationContext)
        {
            var formData = await client.GetAntiforgeryTokenFormAsync(authenticationContext);
            formData.Add(authenticationContext.EmailKey, authenticationContext.EmailValue);
            formData.Add(authenticationContext.PasswordKey, authenticationContext.PasswordValue);

            var response = await client.PostAsync(authenticationContext.LoginUri, new FormUrlEncodedContent(formData));
            response.StatusCode.Should().Be(HttpStatusCode.Redirect);

            SetCookieHeaderValue authenticationCookie = null;
            if (response.Headers.TryGetValues("Set-Cookie", out var values))
                authenticationCookie = SetCookieHeaderValue.ParseList(values.ToList()).SingleOrDefault(c => c.Name.StartsWith(".AspNetCore.Cookies", StringComparison.InvariantCultureIgnoreCase));

            authenticationCookie.Should().NotBeNull();

            client.DefaultRequestHeaders.Add("Cookie", new CookieHeaderValue(authenticationCookie.Name, authenticationCookie.Value).ToString());
        }
    }
}
