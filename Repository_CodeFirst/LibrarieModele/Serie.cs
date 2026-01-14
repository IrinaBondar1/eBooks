using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarieModele
{
    [Table("Serie")]
    public class Serie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_serie { get; set; }

        [Required]
        [MaxLength(100)]
        public string nume_serie { get; set; }

        [MaxLength(300)]
        public string descriere { get; set; }

        public virtual ICollection<Carte> Carti { get; set; }

        public Serie()
        {
            Carti = new HashSet<Carte>();
        }
    }
}
