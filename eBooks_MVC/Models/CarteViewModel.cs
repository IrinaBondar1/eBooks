using Repository_CodeFirst;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace eBooks_MVC.Models
{
    public class CarteViewModel
    {
        public int id_carte { get; set; }

        [Required]
        [Display(Name = "Titlu Carte")]
        public string titlu { get; set; }

        [Display(Name = "Descriere")]
        public string descriere { get; set; }

        [Required]
        [Display(Name = "Autor")]
        public int id_autor { get; set; }

        [Display(Name = "Nume Autor")]
        public string nume_autor { get; set; }

        public IEnumerable<SelectListItem> Autori { get; set; }

        public CarteViewModel()
        {
            Autori = new List<SelectListItem>();
        }

        public CarteViewModel(eBooksContext context)
        {
            Autori = context.Autori
                .Select(a => new SelectListItem
                {
                    Text = a.nume_autor,
                    Value = a.id_autor.ToString()
                })
                .ToList();
        }
    }
}
