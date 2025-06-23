﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.ValueObjects;

public class AuditInfo
{
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}