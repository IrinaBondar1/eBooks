using Repository_CodeFirst;
using LibrarieModele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace NivelAccessDate
{
    public class SerieAccessor
    {
        public List<Serie> GetAll()
        {
            using (var ctx = new eBooksContext())
            {
                return ctx.Serii.ToList();
            }
        }

        public Serie GetById(int id)
        {
            using (var ctx = new eBooksContext())
            {
                return ctx.Serii.Find(id);
            }
        }

        public void Add(Serie serie)
        {
            using (var ctx = new eBooksContext())
            {
                ctx.Serii.Add(serie);
                ctx.SaveChanges();
            }
        }

        public void Update(Serie serie)
        {
            using (var ctx = new eBooksContext())
            {
                ctx.Entry(serie).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var ctx = new eBooksContext())
            {
                var serie = ctx.Serii.Find(id);
                if (serie != null)
                {
                    ctx.Serii.Remove(serie);
                    ctx.SaveChanges();
                }
            }
        }
    }
}
