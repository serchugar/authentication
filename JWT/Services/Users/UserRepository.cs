using JWT.Data;
using Microsoft.EntityFrameworkCore;
using Serchugar.Base.Backend;
using Serchugar.Base.Shared;
using Shared.Entities.User;

namespace JWT.Services.Users;

public class UserRepository(AppDbContext context, string singular = "User", string plural = "Users")
    : BaseRepository<UserModel>(context, singular, plural)
{
    public async Task<Response<IEnumerable<UserModel>>> GetAllAsync() => 
        await base.GetAllAsync();
    
    public async Task<Response<UserModel>> GetByNameExactAsync(string username)
    {
        Response<IEnumerable<UserModel>> result = await base.GetByFilterAsync(user => EF.Functions.ILike(user.Username, username));
        if (result.Code.IsError()) return result.MapErrorResponse<UserModel>();
        
        UserModel? user = result.Data!.FirstOrDefault();
        if (user is null) return Response<UserModel>.FromError(ResponseCodes.NotFound, $"{singular} not found");
        
        return Response<UserModel>.FromSuccess(ResponseCodes.Success, user);
    }

    public async Task<Response<UserModel>> UpdateAsync(string username, UserModel user)
    {
        Response<UserModel> result = await GetByNameExactAsync(username);
        if (result.Code.IsError()) return result.MapErrorResponse<UserModel>();

        Response<bool> newNameIsTaken = await CheckIfUserExistsAsync(user.Username);
        if (newNameIsTaken.Code.IsError()) return result.MapErrorResponse<UserModel>();
        if (newNameIsTaken.Data && user.Username != username) return Response<UserModel>.FromError(ResponseCodes.Conflict, "Username already taken");
        
        UserModel updatedUser = result.Data!;
        updatedUser.Username = user.Username;
        updatedUser.Role = user.Role;
        updatedUser.AuditInfo.UpdatedAt = DateTime.UtcNow;
        
        return await base.UpdateAsync(updatedUser);
    }

    public async Task<Response<UserModel>> CreateAsync(UserModel user)
    {
        Response<bool> result = await CheckIfUserExistsAsync(user.Username);
        if (result.Code.IsError()) return result.MapErrorResponse<UserModel>();
        
        if (result.Data) return Response<UserModel>.FromError(ResponseCodes.Conflict, $"{singular} already exists");
        return await base.CreateAsync(user);
    }
    
    public async Task<Response<UserModel>> DeleteAsync(string username)
    {
        Response<UserModel> result = await GetByNameExactAsync(username);
        if (result.Code.IsError()) return result;
        
        return await base.DeleteAsync(result.Data!);
    }

    public async Task<Response<bool>> CheckIfUserExistsAsync(string username) =>
        await base.CheckIfEntityExistsAsync(user => EF.Functions.ILike(user.Username, username));       
    
}