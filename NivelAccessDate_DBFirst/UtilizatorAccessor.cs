using System.Collections.Generic;
using System.Linq;
using LibrarieModele;
using Repository_CodeFirst;
using System.Data.Entity;

namespace NivelAccessDate
{
    public class UtilizatorAccessor
    {
        public List<Utilizator> GetAll()
        {
            using (var ctx = new eBooksContext())
            {
                return ctx.Utilizatori.ToList();
            }
        }

        public Utilizator GetById(int id)
        {
            using (var ctx = new eBooksContext())
            {
                return ctx.Utilizatori.Find(id);
            }
        }

        public void Add(Utilizator utilizator)
        {
            using (var ctx = new eBooksContext())
            {
                ctx.Utilizatori.Add(utilizator);
                ctx.SaveChanges();
            }
        }

        public void Update(Utilizator utilizator)
        {
            using (var ctx = new eBooksContext())
            {
                ctx.Entry(utilizator).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var ctx = new eBooksContext())
            {
                var utilizator = ctx.Utilizatori.Find(id);
                if (utilizator != null)
                {
                    ctx.Utilizatori.Remove(utilizator);
                    ctx.SaveChanges();
                }
            }
        }
    }
}
