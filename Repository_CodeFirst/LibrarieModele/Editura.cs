using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarieModele
{
    public class Editura
    {
        [Key]
        public int id_editura { get; set; }

        [Required]
        [StringLength(100)]
        public string nume_editura { get; set; }

        [StringLength(50)]
        public string oras { get; set; }
    }
}
