namespace Repository_CodeFirst.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Repository_CodeFirst.eBooksContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Repository_CodeFirst.eBooksContext context)
        {
            if (!context.TipAbonamente.Any())
            {
                context.TipAbonamente.AddOrUpdate(
                    new LibrarieModele.TipAbonament
                    {
                        id_tip_abonament = 1,
                        denumire = "Free",
                        limita_carti_pe_luna = 3,
                        acces_serii_complete = false,
                        permite_descarcare = false
                    },
                    new LibrarieModele.TipAbonament
                    {
                        id_tip_abonament = 2,
                        denumire = "Premium",
                        limita_carti_pe_luna = 10,
                        acces_serii_complete = true,
                        permite_descarcare = false
                    },
                    new LibrarieModele.TipAbonament
                    {
                        id_tip_abonament = 3,
                        denumire = "VIP",
                        limita_carti_pe_luna = int.MaxValue, // Nelimitat
                        acces_serii_complete = true,
                        permite_descarcare = true
                    }
                );
            }

            
            if (!context.Autori.Any())
            {
                context.Autori.AddOrUpdate(
                    new LibrarieModele.Autor { nume_autor = "Ion Creangă" },
                    new LibrarieModele.Autor { nume_autor = "Mihai Eminescu" }
                );
            }

            context.SaveChanges();
        }
    }
}
