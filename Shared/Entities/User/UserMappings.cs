using Riok.Mapperly.Abstractions;
using Serchugar.Base.Shared;

namespace Shared.Entities.User;

[Mapper]
public static partial class UserMappings
{
    // Only RequestDTO -> Model are necessary
    // public static partial UserRequestDTO MapToRequest(this UserModel source);
    public static partial UserModel Map(this UserRequestDTO source);
    // public static partial IEnumerable<UserRequestDTO> MapToRequest(this IEnumerable<UserModel> source);
    public static partial IEnumerable<UserModel> Map(this IEnumerable<UserRequestDTO> source);
    // public static partial Response<UserRequestDTO> MapToRequest(this Response<UserModel> source);
    // public static partial Response<IEnumerable<UserRequestDTO>> MapToRequest(this Response<IEnumerable<UserModel>> source);
    
    // UserDTO <-> Model
    public static partial UserDTO Map(this UserModel source);
    public static partial UserModel Map(this UserDTO source);
    public static partial IEnumerable<UserDTO> Map(this IEnumerable<UserModel> source);
    public static partial IEnumerable<UserModel> Map(this IEnumerable<UserDTO> source);
    public static partial Response<UserDTO> Map(this Response<UserModel> source);
    public static partial Response<IEnumerable<UserDTO>> Map(this Response<IEnumerable<UserModel>> source);
}