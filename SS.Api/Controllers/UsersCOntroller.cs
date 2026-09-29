using SS.Api.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace SS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersCOntroller : ControllerBase
{
    private readonly UserStore _userStore;
    public UsersCOntroller(UserStore userStore)
    {
        _userStore = userStore;
    }

    [HttpPost]
    public async Task<IActionResult> Create()
    {
        var subject = JwtSubjectReader.GetSubject(Request);
        if (subject is null)
        {
            return Unauthorized("Missing or invalid bearer token.");
        }
        {
            var user = await _userStore.Create(subject);
            return Ok(new { user.Id, user.Shape, user.UploadCount });
        }
    }
        
    [HttpGet("{id}")]
    
    public async Task<IActionResult> Get(Guid id)
    {
        var subject = JwtSubjectReader.GetSubject(Request);
        if (subject is null)
        {
            return Unauthorized("Missing or invalid bearer token.");
        }
        
        var user = await _userStore.Get(id, subject);
        return user is null
            ? NotFound()
            : Ok(new { user.Id, user.Shape, user.UploadCount });
    }
}