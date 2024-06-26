﻿using Microsoft.AspNetCore.Http;
using Serilog.Sinks.LogBee.ContextProperties;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Serilog.Sinks.LogBee.AspNetCore
{
    internal static class AspNetCoreHelpers
    {
        public static readonly Lazy<IntegrationClient> IntegrationClient =
            new Lazy<IntegrationClient>(() =>
            {
                return InternalHelpers.GetIntegrationClient(typeof(AspNetCoreHelpers).Assembly);
            });

        public static bool CanReadRequestBody(IHeaderDictionary requestHeaders, LogBeeSinkAspNetCoreConfiguration config)
        {
            string? contentType = requestHeaders?.FirstOrDefault(p => string.Compare(p.Key, "Content-Type", StringComparison.OrdinalIgnoreCase) == 0).Value;
            if (string.IsNullOrEmpty(contentType))
                return false;

            contentType = contentType.Trim().ToLowerInvariant();

            return config.ReadRequestBodyContentTypes?.Any(p => contentType.Contains(p)) == true;
        }

        public static bool CanReadResponseBody(IHeaderDictionary responseHeaders, LogBeeSinkAspNetCoreConfiguration config)
        {
            string? contentType = responseHeaders?.FirstOrDefault(p => string.Compare(p.Key, "Content-Type", StringComparison.OrdinalIgnoreCase) == 0).Value;
            if (string.IsNullOrEmpty(contentType))
                return false;

            contentType = contentType.Trim().ToLowerInvariant();

            return config.ReadResponseBodyContentTypes?.Any(p => contentType.Contains(p)) == true;
        }

        public static string? ReadStreamAsString(Stream stream, Encoding encoding)
        {
            if (stream == null || !stream.CanRead)
                return null;

            string? content = null;
            using (StreamReader reader = new StreamReader(stream, encoding, true))
            {
                stream.Position = 0;
                content = reader.ReadToEnd();
            }

            return content;
        }

        public static string GetResponseFileName(IHeaderDictionary responseHeaders)
        {
            string defaultValue = "Response.txt";
            if (responseHeaders == null)
                return defaultValue;

            string contentType = responseHeaders.FirstOrDefault(p => string.Compare(p.Key, "Content-Type", StringComparison.OrdinalIgnoreCase) == 0).Value.ToString();
            contentType = contentType.ToLowerInvariant();

            if (contentType.Contains("/json"))
                return "Response.json";

            if (contentType.Contains("/xml"))
                return "Response.xml";

            if (contentType.Contains("/html"))
                return "Response.html";

            return defaultValue;
        }
    }
}
