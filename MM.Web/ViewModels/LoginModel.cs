using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Pspcl.ViewModels
{
    public class LoginModel
    {

        [Required(ErrorMessage = "Please enter your username" )]
        [DisplayName("UserName")]
        
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }
        public bool? IsAuthenticated { get; set; }

        public string? ReturnUrl { get; set; }

    }
}
