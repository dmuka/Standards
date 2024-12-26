namespace Domain.Models.Users;

public class User : BaseEntity
{
    //public int PersonId { get; set; }

    //public Person Person { get; set; } = null!;

    public required string UserName { get; set; }

    public required string Email { get; set; }

    public bool IsEmailConfirmed { get; set; } 

    public string? RefreshToken { get; set; }

    public string? AccessToken { get; set; }

    public byte[]? PasswordHash { get; set; } 

    public byte[]? PasswordSalt { get; set; }

    public bool IsTwoFactorEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    public DateTime? LockOutEnd { get; set; }

    public bool IsLockOutEnabled { get; set; }
}