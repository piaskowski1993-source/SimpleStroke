using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace SS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpGet("whoami")]
    public IActionResult WhoAmI()
    {
        var authHeader = Request.Headers.Authorization.ToString();
        
        if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            return Unauthorized("Missing or malformed Authorization header. Expected: Bearer <toker>");
        }
        var token = authHeader["Bearer ".Length..];

        var handler = new JwtSecurityTokenHandler();
        JwtSecurityToken jwt;
        try
        {
            jwt = handler.ReadJwtToken(token);
        }
        catch (Exception)
        {
            return BadRequest("Token could not be read - not a calid JWT.");
        }
        
        var claims = jwt.Claims.Select(c => new { c.Type, c.Value });

        return Ok(claims);
    }
}