using LibrarieModele;

using Repository_CodeFirst;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NivelAccessDate
{
    public class EdituraAccessor
    {
        public List<Editura> GetAll()
        {
            using (var ctx = new eBooksContext())
            {
                return ctx.Edituri.ToList();
            }
        }

        public void Add(Editura editura)
        {
            using (var ctx = new eBooksContext())
            {
                ctx.Edituri.Add(editura);
                ctx.SaveChanges();
            }
        }
    }
}
    

