using System.Collections.Generic;
using System.Linq;
using LibrarieModele;
using Repository_CodeFirst;
using System.Data.Entity;

namespace NivelAccessDate
{
    public class AutorAccessor
    {
        public List<Autor> GetAll()
        {
            using (var ctx = new eBooksContext())
            {
                return ctx.Autori.Where(a => !a.IsDeleted).ToList();
            }
        }

        public Autor GetById(int id)
        {
            using (var ctx = new eBooksContext())
            {
                var autor = ctx.Autori.Find(id);
                return autor != null && !autor.IsDeleted ? autor : null;
            }
        }

        public void Add(Autor autor)
        {
            using (var ctx = new eBooksContext())
            {
                ctx.Autori.Add(autor);
                ctx.SaveChanges();
            }
        }

        public void Update(Autor autor)
        {
            using (var ctx = new eBooksContext())
            {
                ctx.Entry(autor).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var ctx = new eBooksContext())
            {
                var autor = ctx.Autori.Find(id);
                if (autor != null)
                {
                    ctx.Autori.Remove(autor);
                    ctx.SaveChanges();
                }
            }
        }

        public void SoftDelete(int id)
        {
            using (var ctx = new eBooksContext())
            {
                var autor = ctx.Autori.Find(id);
                if (autor != null)
                {
                    autor.IsDeleted = true;
                    ctx.SaveChanges();
                }
            }
        }
    }
}