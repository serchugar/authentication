using JWT.Data;
using Microsoft.EntityFrameworkCore;
using Serchugar.Base.Backend;
using Serchugar.Base.Shared;
using Shared.Entities.User;

namespace JWT.Services.Users;

public class UserRepository(AppDbContext context, string singular = "User", string plural = "Users")
    : BaseRepository<User>(context, singular, plural)
{
    public async Task<Response<IEnumerable<User>>> GetAllAsync() => 
        await base.GetAllAsync();
    
    public async Task<Response<User>> GetByNameExactAsync(string username)
    {
        Response<IEnumerable<User>> result = await base.GetByFilterAsync(user => EF.Functions.ILike(user.Username, username));
        if (result.Code.IsError()) return result.MapErrorResponse<User>();
        
        User? user = result.Data!.FirstOrDefault();
        if (user is null) return Response<User>.FromError(ResponseCodes.NotFound, $"{singular} not found");
        
        return Response<User>.FromSuccess(ResponseCodes.Success, user);
    }

    public async Task<Response<User>> CreateAsync(User userModel) =>
        await base.CreateAsync(userModel);
    
    public async Task<Response<User>> DeleteAsync(string username)
    {
        Response<User> result = await GetByNameExactAsync(username);
        if (result.Code.IsError()) return result;
        
        return await base.DeleteAsync(result.Data!);
    }

    public async Task<Response<bool>> CheckIfUserExistsAsync(string username) =>
        await base.CheckIfEntityExistsAsync(user => EF.Functions.ILike(user.Username, username));       
    
}