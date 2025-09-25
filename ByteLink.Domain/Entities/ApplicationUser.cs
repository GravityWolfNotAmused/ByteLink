using System.ComponentModel.DataAnnotations;

namespace ByteLink.Domain.Entities;

public class ApplicationUser
{
    public long Id { get; set; }
    public string? UserId { get; set; }

    [EmailAddress]
    public required string Email { get; set; }

    public required string PasswordHash { get; set; }

    public required string DatabaseName { get; set; }

    public required string DatabaseUser { get; set; }

    public required string DatabasePWD { get; set; }

    public DateTime CreatedAt { get; set; }
}
