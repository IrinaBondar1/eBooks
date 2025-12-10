using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eBooks_API.Models
{
    public class AutorUpdateDto
    {
        public string nume_autor { get; set; }
        public string tara_origine { get; set; }
        public string data_nasterii { get; set; }
    }
}