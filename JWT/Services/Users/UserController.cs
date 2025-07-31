using Microsoft.AspNetCore.Mvc;
using Serchugar.Base.Backend;
using Serchugar.Base.Shared;
using Shared.Entities.User;

namespace JWT.Services.Users;

[ApiController]
[Route("api/[controller]")]
public class UserController(UserRepository repo) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll() => 
        SetResponse((await repo.GetAllAsync()).Map());
    
    [HttpGet("{username}")]
    public async Task<ActionResult<UserDTO>> GetByNameExact(string username) =>
        SetResponse((await repo.GetByNameExactAsync(username)).Map());

    [HttpDelete("{username}")]
    public async Task<ActionResult<UserDTO>> Delete(string username) => 
        SetResponse((await repo.DeleteAsync(username)).Map());

    [HttpPut("{username}")]
    public async Task<ActionResult<UserDTO>> Update(string username, UserDTO user) =>
        SetResponse((await repo.UpdateAsync(username, user.Map())).Map());
}