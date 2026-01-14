using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarieModele;
using System.Data.Entity;

namespace Repository_CodeFirst
{
    public class eBooksContext : DbContext, IeBooksContext
    {
        public eBooksContext() : base("eBooks")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Autor> Autori { get; set; }
        public DbSet<Carte> Carti { get; set; }
        public DbSet<TipAbonament> TipAbonamente { get; set; }
        public DbSet<Categorie> Categorii { get; set; }
        public DbSet<IstoricCitire> IstoricCitiri { get; set; }
        public DbSet<Serie> Serii { get; set; }
        public DbSet<Utilizator> Utilizatori { get; set; }
        public DbSet<Editura> Edituri { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}
