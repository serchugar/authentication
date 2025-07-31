using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Entities.User;

[NotMapped]
public class UserDTO
{
    [Key]
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public UserRole Role { get; set; }
}