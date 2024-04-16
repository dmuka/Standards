namespace Standards.Core.Models.Users
{
    public class User
    {
        public int Id { get; set; }

        //public int PersonId { get; set; }

        //public Person Person { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public bool IsEmailConfirmed { get; set; }

        public string? RefreshToken { get; set; }

        public string? AccessToken { get; set; }

        public byte[] PasswordHash { get; set; } = null!;

        public byte[] PasswordSalt { get; set; } = null!;

        public bool IsTwoFactorEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        public DateTime LockOutEnd { get; set; }

        public bool IsLockOutEnabled { get; set; }
    }
}