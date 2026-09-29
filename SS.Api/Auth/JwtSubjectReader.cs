using System.IdentityModel.Tokens.Jwt;

namespace SS.Api.Auth;

public static class JwtSubjectReader
{
    public static string? GetSubject(HttpRequest request)
    {
        var authHeader = request.Headers.Authorization.ToString();

        if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            return null;
        }
        
        var token = authHeader["Bearer ".Length..];
        
        try
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return jwt.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        }
        
        catch
        {
            return null;
        }

    }

}