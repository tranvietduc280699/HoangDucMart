using System.ComponentModel.DataAnnotations;

namespace UraDataModel.Model.Users
{
    public class User : BaseEntity
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }
        public string? AvatarUrl { get; set; }
        public bool IsActive { get; set; } 
        public DateTime? LastLoginAt { get; set; }
    }
}
