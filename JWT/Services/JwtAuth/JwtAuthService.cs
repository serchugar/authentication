using JWT.Services.Users;
using Microsoft.AspNetCore.Identity;
using Serchugar.Base.Shared;
using Shared.Entities.User;

namespace JWT.Services.JwtAuth;

public class JwtAuthService(UserRepository users)
{
    public async Task<Response<UserDTO>> RegisterAsync(UserRequestDTO request)
    {
        Response<bool> result = await users.CheckIfUserExistsAsync(request.Username);
        if (result.Code.IsError()) result.MapErrorResponse<UserRequestDTO>();

        bool userExists = result.Data;
        if (userExists) return Response<UserDTO>.FromError(ResponseCodes.Conflict, "Username already exists");

        UserModel newUser = new();
        string hashedPassword = new PasswordHasher<UserModel>().HashPassword(newUser, request.Password);
        
        newUser.Username = request.Username;
        newUser.PasswordHash = hashedPassword;
        
        return (await users.CreateAsync(newUser)).Map();
    }

    public async Task<Response<string>> LoginAsync(UserRequestDTO request)
    {
        Response<UserModel> result = await users.GetByNameExactAsync(request.Username);
        if(result.Code == ResponseCodes.NotFound) return Response<string>.FromError(ResponseCodes.BadRequest, "Invalid username or password");
        if(result.Code.IsError()) result.MapErrorResponse<Response<string>>();
        
        UserModel userModel = result.Data!;
        if(new PasswordHasher<UserModel>().VerifyHashedPassword(userModel, userModel.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            return Response<string>.FromError(ResponseCodes.BadRequest, "Invalid username or password");

        return Response<string>.FromSuccess(ResponseCodes.Success, $"Login successful! Welcome back, {userModel.Username}");
    }
}