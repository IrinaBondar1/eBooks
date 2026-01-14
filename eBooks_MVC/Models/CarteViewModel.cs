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

        [Display(Name = "Serie")]
        public string nume_serie { get; set; }

        [Display(Name = "Numar Volum")]
        public int? nr_volum { get; set; }

        [Display(Name = "Categorie")]
        public string nume_categorie { get; set; }

        [Display(Name = "Categorie")]
        public int? id_categorie { get; set; }

        [Display(Name = "Serie")]
        public int? id_serie { get; set; }

        public IEnumerable<SelectListItem> Autori { get; set; }
        public IEnumerable<SelectListItem> Categorii { get; set; }
        public IEnumerable<SelectListItem> Serii { get; set; }

        public CarteViewModel()
        {
            Autori = new List<SelectListItem>();
            Categorii = new List<SelectListItem>();
            Serii = new List<SelectListItem>();
        }

        public CarteViewModel(eBooksContext context)
        {
            Autori = context.Autori
                .Where(a => !a.IsDeleted)
                .Select(a => new SelectListItem
                {
                    Text = a.nume_autor,
                    Value = a.id_autor.ToString()
                })
                .ToList();

            Categorii = context.Categorii
                .Where(c => !c.IsDeleted)
                .Select(c => new SelectListItem
                {
                    Text = c.denumire,
                    Value = c.id_categorie.ToString()
                })
                .ToList();

            Serii = context.Serii
                .Select(s => new SelectListItem
                {
                    Text = s.nume_serie,
                    Value = s.id_serie.ToString()
                })
                .ToList();
        }
    }
}
