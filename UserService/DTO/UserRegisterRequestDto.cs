using System.ComponentModel.DataAnnotations;
using UserService.Model;

namespace UserService.DTO
{
    public class UserRegisterRequestDto
    {
        public string Email { get; set; } = string.Empty;  
        public string PasswordHash { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public int? RoleId { get; set; }
    }
}
