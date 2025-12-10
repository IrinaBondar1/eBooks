using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarieModele
{
    [Table("Autor")]
    public class Autor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_autor { get; set; }

        [Required]
        [StringLength(200)]
        public string nume_autor { get; set; }
        public DateTime data_nasterii { get; set; }
        public string tara_origine { get; set; }

        public virtual ICollection<Carte> Cartes { get; set; }

        public bool IsDeleted { get; set; }= false;

        public Autor()
        {
            Cartes = new HashSet<Carte>();
        }
    }
}
