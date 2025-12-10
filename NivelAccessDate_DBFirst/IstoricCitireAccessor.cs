using Repository_DBFirst;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NivelAccessDate
{
    public class IstoricCitireAccessor
    {
        public List<IstoricCitire> GetAll()
        {
            using (var ctx = new eBooksEntities())
            {
                return ctx.IstoricCitires.ToList();
            }
        }

        public IstoricCitire GetById(int id)
        {
            using (var ctx = new eBooksEntities())
            {
                return ctx.IstoricCitires.Find(id);
            }
        }

        public void Add(IstoricCitire istoric)
        {
            using (var ctx = new eBooksEntities())
            {
                ctx.IstoricCitires.Add(istoric);
                ctx.SaveChanges();
            }
        }

        public void Update(IstoricCitire istoric)
        {
            using (var ctx = new eBooksEntities())
            {
                ctx.Entry(istoric).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var ctx = new eBooksEntities())
            {
                var istoric = ctx.IstoricCitires.Find(id);
                if (istoric != null)
                {
                    ctx.IstoricCitires.Remove(istoric);
                    ctx.SaveChanges();
                }
            }
        }
    }
}
