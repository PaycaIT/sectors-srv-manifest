using sectors_srv_manifest.AuthModule.Exceptions;
using sectors_srv_manifest.AuthModule.Models;
using System.Security.Claims;


namespace sectors_service_orders.Auth;

public static class JWTUtils
{
    public static bool ValidateBussinesData(IEnumerable<Claim>? userClaims)
    {
        string? clientId = userClaims?.FirstOrDefault(c => c.Type == "clientId")?.Value;
        string? userId = userClaims?.FirstOrDefault(c => c.Type == "userId")?.Value;

        if (clientId == null || userId == null) { return false; }
        try
        {
            int.Parse(clientId);
        }
        catch
        {
            return false;
        }

        return true;
    }

    public static JwtModel GetAuthData(IEnumerable<Claim> userClaims)
    {
        JwtModel jwt = new JwtModel()
        {
            ClientId = GetClientId(userClaims),
            UserId = GetUserId(userClaims)
        };

        return jwt;
    }

    /// <summary>
    /// Reads the clientId from the JWT Token
    /// </summary>
    /// <param name="userClaims">The list of user claims given by theb JWT</param>
    /// <returns>The clientId of the JWT as an int</returns>
    private static int GetClientId(IEnumerable<Claim> userClaims)
    {
        string? clientId = userClaims.FirstOrDefault(c => c.Type == "clientId")?.Value;
        if (clientId == null)
        {
            throw new AuthException("Invalid JWT");
        }
        return int.Parse(clientId);

    }

    private static string GetUserId(IEnumerable<Claim> userClaims)
    {
        string? userId = userClaims.FirstOrDefault(c => c.Type == "userId")?.Value;
        if (userId == null)
        {
            throw new AuthException("Invalid JWT");
        }
        return userId;
    }
}
