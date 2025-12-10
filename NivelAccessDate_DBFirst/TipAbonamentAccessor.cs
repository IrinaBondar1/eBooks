using Repository_DBFirst;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;

namespace NivelAccessDate
{
    public class TipAbonamentAccessor
    {
        public List<TipAbonament> GetAll()
        {
            using (var ctx = new eBooksEntities())
            {
                return ctx.TipAbonaments.ToList();
            }
        }

        public TipAbonament GetById(int id)
        {
            using (var ctx = new eBooksEntities())
            {
                return ctx.TipAbonaments.Find(id);
            }
        }

        public void Add(TipAbonament tip)
        {
            using (var ctx = new eBooksEntities())
            {
                ctx.TipAbonaments.Add(tip);
                ctx.SaveChanges();
            }
        }

        public void Update(TipAbonament tip)
        {
            using (var ctx = new eBooksEntities())
            {
                ctx.Entry(tip).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var ctx = new eBooksEntities())
            {
                var tip = ctx.TipAbonaments.Find(id);
                if (tip != null)
                {
                    ctx.TipAbonaments.Remove(tip);
                    ctx.SaveChanges();
                }
            }
        }
    }
}
