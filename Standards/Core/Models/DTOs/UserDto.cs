namespace Standards.Core.Models.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public string? Email { get; set; }
    }
}
