using System.ComponentModel.DataAnnotations;

namespace eBooks_MVC.Models
{
    public class CategorieViewModel
    {
        public int id_categorie { get; set; }

        [Required]
        [Display(Name = "Denumire Categorie")]
        [StringLength(100)]
        public string denumire { get; set; }

        [Display(Name = "Descriere")]
        [StringLength(300)]
        public string descriere { get; set; }
    }
}

