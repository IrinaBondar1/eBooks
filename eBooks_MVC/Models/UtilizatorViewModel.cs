using System;
using System.ComponentModel.DataAnnotations;

namespace eBooks_MVC.Models
{
    public class UtilizatorViewModel
    {
        public int id_utilizator { get; set; }

        [Display(Name = "Nume utilizator")]
        public string nume_utilizator { get; set; }

        [Display(Name = "Email")]
        public string email { get; set; }

        [Display(Name = "Data inregistrare")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime data_inregistrare { get; set; }

        [Display(Name = "Plan abonament")]
        public string denumire_plan { get; set; }

        [Display(Name = "Carti citite luna curenta")]
        public int carti_citite_luna { get; set; }

        [Display(Name = "Limita carti/luna")]
        public int limita_carti_pe_luna { get; set; }
    }
}
