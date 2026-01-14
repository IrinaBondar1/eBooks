using System.Collections.Generic;
using System.Linq;
using LibrarieModele;
using Repository_CodeFirst;
using System.Data.Entity;

namespace NivelAccessDate
{
    public class UtilizatorAccessor
    {
        public List<Utilizator> GetAll()
        {
            using (var ctx = new eBooksContext())
            {
                return ctx.Utilizatori.ToList();
            }
        }

        public Utilizator GetById(int id)
        {
            using (var ctx = new eBooksContext())
            {
                return ctx.Utilizatori
                    .Include("TipAbonament")
                    .FirstOrDefault(u => u.id_utilizator == id);
            }
        }

        public Utilizator GetByEmail(string email)
        {
            using (var ctx = new eBooksContext())
            {
                return ctx.Utilizatori
                    .Include("TipAbonament")
                    .FirstOrDefault(u => u.email == email);
            }
        }

        public Utilizator GetByEmailAndPassword(string email, string parola)
        {
            using (var ctx = new eBooksContext())
            {
                return ctx.Utilizatori
                    .Include("TipAbonament")
                    .FirstOrDefault(u => u.email == email && u.parola == parola);
            }
        }

        public void Add(Utilizator utilizator)
        {
            using (var ctx = new eBooksContext())
            {
                ctx.Utilizatori.Add(utilizator);
                ctx.SaveChanges();
            }
        }

        public void Update(Utilizator utilizator)
        {
            if (utilizator == null)
                return;

            using (var ctx = new eBooksContext())
            {
                // Incarca entitatea din context pentru a evita problemele cu relatii
                var utilizatorDb = ctx.Utilizatori.FirstOrDefault(u => u.id_utilizator == utilizator.id_utilizator);
                if (utilizatorDb == null)
                {
                    throw new System.ArgumentException($"Utilizator cu ID {utilizator.id_utilizator} nu a fost gasit.");
                }

                // Actualizeaza doar proprietatile necesare, nu intreaga entitate
                utilizatorDb.nume_utilizator = utilizator.nume_utilizator;
                utilizatorDb.email = utilizator.email;
                utilizatorDb.parola = utilizator.parola;
                utilizatorDb.id_tip_abonament = utilizator.id_tip_abonament;
                utilizatorDb.carti_citite_luna = utilizator.carti_citite_luna;
                utilizatorDb.data_inregistrare = utilizator.data_inregistrare;
                
                ctx.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var ctx = new eBooksContext())
            {
                var utilizator = ctx.Utilizatori.Find(id);
                if (utilizator != null)
                {
                    ctx.Utilizatori.Remove(utilizator);
                    ctx.SaveChanges();
                }
            }
        }
    }
}
