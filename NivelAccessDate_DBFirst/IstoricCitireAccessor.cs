using Repository_CodeFirst;
using LibrarieModele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace NivelAccessDate
{
    public class IstoricCitireAccessor
    {
        public List<IstoricCitire> GetAll()
        {
            using (var ctx = new eBooksContext())
            {
                return ctx.IstoricCitiri
                    .Include("Utilizator")
                    .Include("Carte")
                    .ToList();
            }
        }

        public IstoricCitire GetById(int id)
        {
            using (var ctx = new eBooksContext())
            {
                return ctx.IstoricCitiri
                    .Include("Utilizator")
                    .Include("Carte")
                    .FirstOrDefault(i => i.id_istoric == id);
            }
        }

        public List<IstoricCitire> GetByUtilizatorId(int idUtilizator)
        {
            using (var ctx = new eBooksContext())
            {
                return ctx.IstoricCitiri
                    .Include("Carte")
                    .Where(i => i.id_utilizator == idUtilizator)
                    .OrderByDescending(i => i.data_accesare)
                    .ToList();
            }
        }

        public void Add(IstoricCitire istoric)
        {
            using (var ctx = new eBooksContext())
            {
                ctx.IstoricCitiri.Add(istoric);
                ctx.SaveChanges();
            }
        }

        public void Update(IstoricCitire istoric)
        {
            using (var ctx = new eBooksContext())
            {
                ctx.Entry(istoric).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var ctx = new eBooksContext())
            {
                var istoric = ctx.IstoricCitiri.Find(id);
                if (istoric != null)
                {
                    ctx.IstoricCitiri.Remove(istoric);
                    ctx.SaveChanges();
                }
            }
        }
    }
}
