using System;
using System.ComponentModel.DataAnnotations;

namespace eBooks_MVC.Models
{
    public class IstoricCitireViewModel
    {
        public int id_istoric { get; set; }

        [Display(Name = "Titlu Carte")]
        public string titlu_carte { get; set; }

        [Display(Name = "Data Accesare")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        public DateTime data_accesare { get; set; }

        [Display(Name = "Actiune")]
        public string actiune { get; set; }
    }
}
