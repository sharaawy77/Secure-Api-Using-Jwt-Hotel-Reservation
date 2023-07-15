using System.ComponentModel.DataAnnotations;

namespace Secure_Api_Jwt.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name ="UserName Or Email")]
        public string Email { get; set; }
        [Required]
        public string PassWord { get; set; }
    }
}
