using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarieModele
{
    [Table("TipAbonament")]
    public class TipAbonament
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_tip_abonament { get; set; }

        [Required]
        [MaxLength(100)]
        public string denumire { get; set; }

        public int limita_carti_pe_luna { get; set; }

        public bool acces_serii_complete { get; set; }

        public bool permite_descarcare { get; set; }

        public virtual ICollection<Utilizator> Utilizatori { get; set; }

        public TipAbonament()
        {
            Utilizatori = new HashSet<Utilizator>();
        }
    }
}
