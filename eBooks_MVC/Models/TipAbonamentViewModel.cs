using System.ComponentModel.DataAnnotations;

namespace eBooks_MVC.Models
{
    public class TipAbonamentViewModel
    {
        public int id_tip_abonament { get; set; }

        [Display(Name = "Denumire Plan")]
        public string denumire { get; set; }

        [Display(Name = "Limita carti/luna")]
        public int limita_carti_pe_luna { get; set; }

        [Display(Name = "Acces serii complete")]
        public bool acces_serii_complete { get; set; }

        [Display(Name = "Permite descarcare")]
        public bool permite_descarcare { get; set; }
    }
}
