using System.Collections.Generic;
using System.Linq;
using LibrarieModele;
using Repository_CodeFirst;
using System.Data.Entity;

namespace NivelAccessDate
{
    public class CategorieAccessor
    {

        public List<Categorie> GetAll()
        {
            using (var ctx = new eBooksContext())
            {
                return ctx.Categorii.ToList();
            }
        }

        public Categorie GetById(int id)
        {
            using (var ctx = new eBooksContext())
            {
                return ctx.Categorii.Find(id);
            }
        }

        public void Add(Categorie categorie)
        {
            using (var ctx = new eBooksContext())
            {
                ctx.Categorii.Add(categorie);
                ctx.SaveChanges();
            }
        }

        public void Update(Categorie categorie)
        {
            using (var ctx = new eBooksContext())
            {
                ctx.Entry(categorie).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var ctx = new eBooksContext())
            {
                var categorie = ctx.Categorii.Find(id);
                if (categorie != null)
                {
                    ctx.Categorii.Remove(categorie);
                    ctx.SaveChanges();
                }
            }
        }
    
}
}
