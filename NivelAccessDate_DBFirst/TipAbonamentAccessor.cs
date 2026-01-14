using Repository_CodeFirst;
using LibrarieModele;
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
            using (var ctx = new eBooksContext())
            {
                return ctx.TipAbonamente.ToList();
            }
        }

        public TipAbonament GetById(int id)
        {
            using (var ctx = new eBooksContext())
            {
                return ctx.TipAbonamente.Find(id);
            }
        }

        public void Add(TipAbonament tip)
        {
            using (var ctx = new eBooksContext())
            {
                ctx.TipAbonamente.Add(tip);
                ctx.SaveChanges();
            }
        }

        public void Update(TipAbonament tip)
        {
            using (var ctx = new eBooksContext())
            {
                ctx.Entry(tip).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var ctx = new eBooksContext())
            {
                var tip = ctx.TipAbonamente.Find(id);
                if (tip != null)
                {
                    ctx.TipAbonamente.Remove(tip);
                    ctx.SaveChanges();
                }
            }
        }
    }
}
