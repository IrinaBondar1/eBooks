using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarieModele
{
    [Table("Utilizator")]
    public class Utilizator
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_utilizator { get; set; }

        [Required]
        [MaxLength(100)]
        public string nume_utilizator { get; set; }

        [Required]
        [MaxLength(100)]
        public string email { get; set; }

        [Required]
        [MaxLength(100)]
        public string parola { get; set; }

        public DateTime data_inregistrare { get; set; }

        [ForeignKey("TipAbonament")]
        public int id_tip_abonament { get; set; }

        public int carti_citite_luna { get; set; }

        public virtual TipAbonament TipAbonament { get; set; }
        public  virtual ICollection<IstoricCitire> IstoricCitiri { get; set; }

        public Utilizator()
        {
            IstoricCitiri = new HashSet<IstoricCitire>();
        }
    }
}
