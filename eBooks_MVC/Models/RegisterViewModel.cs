using System.ComponentModel.DataAnnotations;

namespace eBooks_MVC.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Numele utilizatorului este obligatoriu")]
        [Display(Name = "Nume utilizator")]
        [StringLength(100, MinimumLength = 3)]
        public string NumeUtilizator { get; set; }

        [Required(ErrorMessage = "Email-ul este obligatoriu")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Parola este obligatorie")]
        [StringLength(100, ErrorMessage = "Parola trebuie sa aiba minim {2} caractere", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string Parola { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirma parola")]
        [Compare("Parola", ErrorMessage = "Parola si confirmarea parolei nu se potrivesc.")]
        public string ConfirmParola { get; set; }

        [Display(Name = "Tip abonament")]
        public int IdTipAbonament { get; set; } = 1; // Default: Free
    }
}
