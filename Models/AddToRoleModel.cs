using System.ComponentModel.DataAnnotations;

namespace Secure_Api_Jwt.Models
{
    public class AddToRoleModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
