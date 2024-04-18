using Standards.Core.Models.Interfaces;

namespace Standards.Core.Models.DTOs
{
    public class UserDto : IEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Email { get; set; }
    }
}
