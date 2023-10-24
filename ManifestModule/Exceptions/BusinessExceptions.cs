namespace sectors_srv_manifest.ManifestModule.Exceptions;

public class ForbiddenClientException : Exception
{
    public ForbiddenClientException(string message) : base(message)
    {
    }
}

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string message) : base(message)
    {
    }
}

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
    }
}

public class OSAlreadyInManifestException : Exception
{
    public string? ManifestClientOrdinalId { get; set; }
    public string? FoundManifestSectorCode { get; set; }
    public OSAlreadyInManifestException(string message, string manifestClientOrdinal = null, string manifestSectorCode = null) : base(message)
    {
        ManifestClientOrdinalId = manifestClientOrdinal;
        FoundManifestSectorCode = manifestSectorCode;
    }
}
