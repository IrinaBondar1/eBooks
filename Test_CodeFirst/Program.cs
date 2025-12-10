using LibrarieModele;
using NivelAccessDate;
using Repository_CodeFirst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Test_CodeFirst
{
    public class Program
    {
        static void Main(string[] args)
        {


            /* EXERCITIUL 4 *

           Console.WriteLine("=== EXERCITIUL 4 \nAfisare date din baza de date eBooks ===\n");

           using (var context = new eBooksContext())
           {
               // 🔹 Afișăm toți autorii existenți
               var autorii = context.Autori.ToList();

               if (autorii.Any())
               {
                   Console.WriteLine("Autori existenti:");
                   foreach (var a in autorii)
                   {
                       Console.WriteLine($"ID: {a.id_autor} | Nume: {a.nume_autor}");
                   }
               }
               else
               {
                   Console.WriteLine("Nu exista autori in baza de date.");
               }

               Console.WriteLine("\n--------------------------------------\n");

               // 🔹 Afișăm tipurile de abonament (dacă vrei)
               var abonamente = context.TipAbonamente.ToList();

               if (abonamente.Any())
               {
                   Console.WriteLine("Tipuri de abonament:");
                   foreach (var t in abonamente)
                   {
                       Console.WriteLine($"{t.denumire} - Limita: {t.limita_carti_pe_luna} carti/luna");
                   }
               }
               else
               {
                   Console.WriteLine("Nu exista tipuri de abonament.");
               }
           }*/


            /* EXERCITIUL 5 *

           var accessor = new AutorAccessor();

           Console.WriteLine("\n=== EXERCITIUL 5 \nLista autorilor din baza de date ===\n");

           var autori = accessor.GetAll();

           if (autori.Count == 0)
           {
               Console.WriteLine("Nu exista autori in baza de date!");
           }
           else
           {
               foreach (var autor in autori)
               {
                   Console.WriteLine($"ID: {autor.id_autor} | Nume: {autor.nume_autor}");
               }
           }
           Console.ReadLine();
            */


            /* EXERCITIUL 6 ORM - EFECTUAREA MODIFICARILOR */

            /*
            var autorAccessor = new AutorAccessor();
            var edituriAccessor = new EdituraAccessor();

            Console.WriteLine("=== Lista autorilor (cu noile proprietati) ===\n");

            var autori = autorAccessor.GetAll();
            foreach (var a in autori)
            {
                Console.WriteLine($"ID: {a.id_autor} | Nume: {a.nume_autor} | Tara: {a.tara_origine} | Nascut: {a.data_nasterii.ToShortDateString()}");
            }

            Console.WriteLine("\n=== Adaugare autor nou ===");
            autorAccessor.Add(new Autor
            {
                nume_autor = "George Calinescu",
                data_nasterii = new DateTime(1899, 6, 19),
                tara_origine = "Romania"
            });
            Console.WriteLine("Autor adaugat cu succes!");

            Console.WriteLine("\n=== Adaugare editura noua ===");
            edituriAccessor.Add(new Editura
            {
                nume_editura = "Humanitas",
                oras = "Bucuresti"
            });
            Console.WriteLine("Editură adaugata cu succes!");

            Console.WriteLine("\n=== Lista editurilor ===\n");
            var edituri = edituriAccessor.GetAll();
            foreach (var e in edituri)
            {
                Console.WriteLine($"ID: {e.id_editura} | Nume: {e.nume_editura} | Oras: {e.oras}");
            }

            Console.WriteLine("\nApasă Enter pentru a închide...");
            Console.ReadLine();
            */
            using (var ctx = new eBooksContext())
            {
                Console.WriteLine("=== Test Lazy Loading ===\n");

                // Se încarcă doar Autorii
                var autori = ctx.Autori.ToList();

                foreach (var autor in autori)
                {
                    Console.WriteLine($"Autor: {autor.nume_autor}");

                    // Aici EF încarcă Cartile doar când ajungi la această linie
                    foreach (var carte in autor.Cartes)
                    {
                        Console.WriteLine($"   Carte: {carte.titlu}");
                    }
                }
            }

            

            using (var ctx = new eBooksContext())
            {
                Console.WriteLine("\n=== Test Eager Loading ===\n");

                // Include() încarcă și Cartile odată cu Autorii
                var autori = ctx.Autori.Include("Cartes").ToList();

                foreach (var autor in autori)
                {
                    Console.WriteLine($"Autor: {autor.nume_autor}");

                    foreach (var carte in autor.Cartes)
                    {
                        Console.WriteLine($"   Carte: {carte.titlu}");
                    }
                }
            }

            Console.ReadLine();
        }
    }
}

