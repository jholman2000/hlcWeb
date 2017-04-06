using System.ComponentModel.DataAnnotations;

namespace hlcWeb.ViewModels
{
    public class LogonViewModel
    {
        public string ErrorMessage { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}