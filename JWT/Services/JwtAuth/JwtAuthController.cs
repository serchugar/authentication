using JWT.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serchugar.Base.Backend;
using Shared.Entities.User;

namespace JWT.Services.JwtAuth;

[ApiController]
[Route("api/[controller]")]
public class JwtAuthController(JwtAuthService service) : BaseController
{
    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> Register([FromBody] UserRequestDTO request) =>
        SetResponse(await service.RegisterAsync(request), true, typeof(UserController));

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] UserRequestDTO request) =>
        SetResponse(await service.LoginAsync(request));
    
    [Authorize]
    [HttpGet("authorized")]
    public ActionResult AuthenticatedOnly() => Ok("Authenticated");
}