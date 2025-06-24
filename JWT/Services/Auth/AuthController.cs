using JWT.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Serchugar.Base.Backend;
using Shared.Entities.User;

namespace JWT.Services.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AuthService service) : BaseController
{
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register([FromBody] UserDTO request) =>
        SetResponse(await service.RegisterAsync(request), true, typeof(UserController));

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] UserDTO request) =>
        SetResponse(await service.LoginAsync(request));
}