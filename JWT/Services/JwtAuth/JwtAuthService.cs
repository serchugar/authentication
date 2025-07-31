using System.Security.Claims;
using System.Text;
using JWT.Services.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Serchugar.Base.Shared;
using Shared.Entities.User;

namespace JWT.Services.JwtAuth;

public class JwtAuthService(UserRepository users, IConfiguration config)
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
        
        UserModel user = result.Data!;
        if(new PasswordHasher<UserModel>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            return Response<string>.FromError(ResponseCodes.BadRequest, "Invalid username or password");

        string token = CreateToken(user);
        return Response<string>.FromSuccess(ResponseCodes.Success, token);
    }
    
    private string CreateToken(UserModel user)
    {
        Dictionary<string, object> claims = new()
        {
            [ClaimTypes.Name] = user.Username,
            [ClaimTypes.NameIdentifier] = user.Id
        };

        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(config["JwtSettings:Token"]!));
        
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha512);

        SecurityTokenDescriptor descriptor = new()
        {
            Issuer = config["JwtSettings:Issuer"],
            Audience = config["JwtSettings:Audience"],
            Claims = claims,
            NotBefore = DateTime.Now,
            Expires = DateTime.Now.AddMinutes(Convert.ToDouble(config["JwtSettings:ExpirationMinutes"])),
            SigningCredentials = credentials
        };
        
        JsonWebTokenHandler handler = new();
        return handler.CreateToken(descriptor);
    }
}