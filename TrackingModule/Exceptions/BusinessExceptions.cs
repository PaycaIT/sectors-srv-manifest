namespace sectors_srv_manifest.TrackingModule.Exceptions;

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

