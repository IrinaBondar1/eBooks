using System.Collections.Generic;
using System.Linq;
using LibrarieModele;
using Repository_CodeFirst;
using System.Data.Entity;

namespace NivelAccessDate
{
    public class CarteAccessor
    {
        public List<Carte> GetAll()
        {
            using (var ctx = new eBooksContext())
            {
                return ctx.Carti.ToList();
            }
        }

        public Carte GetById(int id)
        {
            using (var ctx = new eBooksContext())
            {
                return ctx.Carti.Find(id);
            }
        }

        public void Add(Carte carte)
        {
            using (var ctx = new eBooksContext())
            {
                ctx.Carti.Add(carte);
                ctx.SaveChanges();
            }
        }

        public void Update(Carte carte)
        {
            using (var ctx = new eBooksContext())
            {
                ctx.Entry(carte).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var ctx = new eBooksContext())
            {
                var carte = ctx.Carti.Find(id);
                if (carte != null)
                {
                    ctx.Carti.Remove(carte);
                    ctx.SaveChanges();
                }
            }
        }
    
    }
}
