using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Opw.AspNetCore.Http
{
    public static class HttpRequestExtensions
    {
        public static Uri GetAbsoluteUri(this HttpRequest httpRequest)
        {
            var uriBuilder = new UriBuilder();
            uriBuilder.Scheme = httpRequest.Scheme;
            uriBuilder.Host = httpRequest.Host.Host;
            if (httpRequest.Host.Port.HasValue) uriBuilder.Port = httpRequest.Host.Port.Value;
            uriBuilder.Path = httpRequest.Path.ToString();
            uriBuilder.Query = httpRequest.QueryString.ToString();
            return uriBuilder.Uri;
        }

        public static bool TryGetOriginDomain(this HttpRequest httpRequest, out string originDomain)
        {
            originDomain = null;

            var originDomainUrl = httpRequest.Headers["Origin"];
            if (string.IsNullOrEmpty(originDomainUrl))
            {
                originDomainUrl = httpRequest.Headers["Referer"];
            }

            Uri originDomainUri;
            var createAttempt = Uri.TryCreate(originDomainUrl, UriKind.RelativeOrAbsolute, out originDomainUri);
            if (!createAttempt)
                return false;

            originDomain = originDomainUri.Host;
            if (!originDomainUri.IsDefaultPort)
                originDomain += ":" + originDomainUri.Port;

            return true;
        }
    }
}
