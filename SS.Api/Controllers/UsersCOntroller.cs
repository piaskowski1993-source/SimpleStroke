using Microsoft.AspNetCore.Mvc;

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
    public IActionResult Create()
    {
        var user = _userStore.Create();
        return Ok(new { user.Id, user.Shape, user.UploadCount });
    }
        
    [HttpGet("{id}")]
    
    public IActionResult Get(Guid id)
    {
        var user = _userStore.Get(id);
        return user is null
        ? NotFound()
        : Ok(new { user.Id, user.Shape, user.UploadCount });

    }
}