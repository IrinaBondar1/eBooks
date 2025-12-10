using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarieModele
{
    [Table("IstoricCitire")]
    public class IstoricCitire
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_istoric { get; set; }

        [ForeignKey("Utilizator")]
        public int id_utilizator { get; set; }

        [ForeignKey("Carte")]
        public int id_carte { get; set; }

        public DateTime data_accesare { get; set; }

        [MaxLength(50)]
        public string actiune { get; set; }

        public  Utilizator Utilizator { get; set; }
        public  Carte Carte { get; set; }
    }
}
