using Repository_DBFirst;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;

namespace NivelAccessDate
{
    public class SerieAccessor
    {
        public List<Serie> GetAll()
        {
            using (var ctx = new eBooksEntities())
            {
                return ctx.Series.ToList();
            }
        }

        public Serie GetById(int id)
        {
            using (var ctx = new eBooksEntities())
            {
                return ctx.Series.Find(id);
            }
        }

        public void Add(Serie serie)
        {
            using (var ctx = new eBooksEntities())
            {
                ctx.Series.Add(serie);
                ctx.SaveChanges();
            }
        }

        public void Update(Serie serie)
        {
            using (var ctx = new eBooksEntities())
            {
                ctx.Entry(serie).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var ctx = new eBooksEntities())
            {
                var serie = ctx.Series.Find(id);
                if (serie != null)
                {
                    ctx.Series.Remove(serie);
                    ctx.SaveChanges();
                }
            }
        }
    }
}
