using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UraDataModel.DTOs.Users
{
    /// <summary>
    /// cập nhật user
    /// </summary>
    public class UserUpdateDto
    {
        [Required]
        public string UserId { get; set; } = default!;
        [Required]
        public string UserName { get; set; } = default!;    

        [Required]
        [StringLength(150)]
        public string FullName { get; set; } = default!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Phone]
        public string? Phone { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
    }
}
