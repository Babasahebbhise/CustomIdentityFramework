using System.ComponentModel.DataAnnotations;

namespace CustomIdentityAuthentication.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="UserName Required.")]
        public string? UserName { get; set; }
        [Required(ErrorMessage ="Password is Required.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Display(Name ="Remember Me")]
        public bool RemeberMe { get; set; }
    }
}
