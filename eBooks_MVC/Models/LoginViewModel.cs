using System.ComponentModel.DataAnnotations;

namespace eBooks_MVC.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email-ul este obligatoriu")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Parola este obligatorie")]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string Parola { get; set; }

        [Display(Name = "Tine-ma minte")]
        public bool RememberMe { get; set; }
    }
}
