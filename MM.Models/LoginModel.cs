using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Pspcl.Models
{
    public class LoginModel
    {

        [Required(ErrorMessage = "Please enter your username")]
        [DisplayName("UserName")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter your password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }
        public bool IsAuthenticated { get; set; }

    }
}
