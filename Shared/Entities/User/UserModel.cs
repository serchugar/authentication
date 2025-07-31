using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Shared.ValueObjects;

namespace Shared.Entities.User;

[Table("users", Schema = "authentication")]
public class UserModel
{
    [Column("id"), Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Column("username"), Required, MaxLength(64)]
    public string Username { get; set; } = null!;
    
    [Column("password_hash"), Required, MaxLength(84)]
    public string PasswordHash { get; set; } = null!;
    
    [Column("role")]
    public UserRole Role { get; set; } = UserRole.User;
    
    
    // Owned tables
    public AuditInfo AuditInfo { get; set; } = new();
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserRole
{
    User,
    Admin,
}