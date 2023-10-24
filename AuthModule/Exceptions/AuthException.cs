namespace sectors_srv_manifest.AuthModule.Exceptions;

public class AuthException : Exception
{
    public AuthException(string v) : base("Invalid credentials")
    {

    }
}
