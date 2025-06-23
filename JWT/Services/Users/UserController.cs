using Microsoft.AspNetCore.Mvc;
using Serchugar.Base.Backend;
using Shared.Entities.User;

namespace JWT.Services.Users;

[ApiController]
[Route("api/[controller]")]
public class UserController(UserRepository repo) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAll() => SetResponse(await repo.GetAllAsync());
    
    [HttpGet("{username}")]
    public async Task<ActionResult<User>> GetByNameExact(string username) => SetResponse(await repo.GetByNameExactAsync(username));

    [HttpDelete("{username}")]
    public async Task<ActionResult<User>> Delete(string username) => SetResponse(await repo.DeleteAsync(username));
}