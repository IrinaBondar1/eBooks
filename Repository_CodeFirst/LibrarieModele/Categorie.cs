using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarieModele
{
    [Table("Categorie")]
    public class Categorie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_categorie { get; set; }

        [Required]
        [MaxLength(100)]
        public string denumire { get; set; }

        [MaxLength(300)]
        public string descriere { get; set; }

        public  virtual ICollection<Carte> Carti { get; set; }

        public Categorie()
        {
            Carti = new HashSet<Carte>();
        }
    }
}
