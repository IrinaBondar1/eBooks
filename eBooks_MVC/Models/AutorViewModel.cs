using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eBooks_MVC.Models
{
    public class AutorViewModel
    {
        public int id_autor { get; set; }

        [Required]
        [Display(Name = "Nume autor")]
        public string nume_autor { get; set; }

        [Display(Name = "Data nasterii")]
        [DataType(DataType.Date)]
        public DateTime data_nasterii { get; set; }

        [Display(Name = "Tara de origine")]
        public string tara_origine { get; set; }

        [Display(Name = "Activ")]
        public bool activ { get; set; } 
    }
}
