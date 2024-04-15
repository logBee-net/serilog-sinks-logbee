﻿namespace Serilog.Sinks.LogBee.AspNetCore;

internal class LogBeeSinkConfiguration
{
    public string OrganizationId { get; init; }
    public string ApplicationId { get; init; }
    public Uri LogBeeUri { get; init; }

    public LogBeeSinkConfiguration(string organizationId, string applicationId, string logBeeEndpoint)
    {
        if (string.IsNullOrWhiteSpace(organizationId))
            throw new ArgumentNullException(nameof(organizationId));

        if (string.IsNullOrWhiteSpace(applicationId))
            throw new ArgumentNullException(nameof(applicationId));

        if (string.IsNullOrWhiteSpace(logBeeEndpoint))
            throw new ArgumentNullException(nameof(logBeeEndpoint));

        if (!Uri.TryCreate(logBeeEndpoint, UriKind.Absolute, out var logBeeUri))
            throw new ArgumentException($"{nameof(logBeeEndpoint)} must be an absolute URI");

        OrganizationId = organizationId;
        ApplicationId = applicationId;
        LogBeeUri = logBeeUri;
    }
}
