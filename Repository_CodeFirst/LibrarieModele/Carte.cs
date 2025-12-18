using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarieModele
{
    [Table("Carte")]
    [Serializable] 
    public class Carte
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_carte { get; set; }

        [Required]
        [MaxLength(150)]
        public string titlu { get; set; }

        [MaxLength(500)]
        public string descriere { get; set; }

        [ForeignKey("Autor")]
        public int id_autor { get; set; }

        public  virtual Autor Autor { get; set; }
    }
}
