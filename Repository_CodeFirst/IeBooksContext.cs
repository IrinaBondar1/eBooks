using LibrarieModele;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_CodeFirst
{
    public interface IeBooksContext
    {
        DbSet<Autor> Autori { get; set; }
        DbSet<Carte> Carti { get; set; }

        int SaveChanges();
        Task<int> SaveChangesAsync(); 
    }
}
