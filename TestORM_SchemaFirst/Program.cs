using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NivelAccessDate_DBFirst;


namespace TestORM_SchemaFirst
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== TEST ORM Database First ===\n");

            // --- Test 1: Cărți ---
            var carteAccessor = new CarteAccessor();
            var carti = carteAccessor.GetAll();

            Console.WriteLine("Lista de carti:");
            foreach (var c in carti)
            {
                Console.WriteLine($"  [{c.id_carte}] {c.titlu}  (AutorID: {c.id_autor}, CategorieID: {c.id_categorie})");
            }

            Console.WriteLine($"\nTotal carti: {carti.Count}\n");


            // --- Test 2: Autori ---
            var autorAccessor = new AutorAccessor();
            var autori = autorAccessor.GetAll();

            Console.WriteLine("Lista de autori:");
            foreach (var a in autori)
            {
                Console.WriteLine($"  [{a.id_autor}] {a.nume_autor}");
            }

            Console.WriteLine($"\nTotal autori: {autori.Count}\n");


            // --- Test 3: Categorii ---
            var categorieAccessor = new CategorieAccessor();
            var categorii = categorieAccessor.GetAll();

            Console.WriteLine("Lista de categorii:");
            foreach (var cat in categorii)
            {
                Console.WriteLine($"  [{cat.id_categorie}] {cat.denumire}");
            }

            Console.WriteLine($"\nTotal categorii: {categorii.Count}\n");


            // --- Test 4: Utilizatori ---
            var utilizatorAccessor = new UtilizatorAccessor();
            var utilizatori = utilizatorAccessor.GetAll();

            Console.WriteLine("Lista de utilizatori:");
            foreach (var u in utilizatori)
            {
                Console.WriteLine($"  [{u.id_utilizator}] {u.nume_utilizator} ({u.email}) - Tip abonament: {u.id_tip_abonament}");
            }

            Console.WriteLine($"\nTotal utilizatori: {utilizatori.Count}\n");


            // --- Test 5: Tipuri de abonamente ---
            var tipAccessor = new TipAbonamentAccessor();
            var abonamente = tipAccessor.GetAll();

            Console.WriteLine("Tipuri de abonament:");
            foreach (var t in abonamente)
            {
                Console.WriteLine($"  [{t.id_tip_abonament}] {t.denumire} - Limita carti/lună: {t.limita_carti_pe_luna}");
            }

            Console.WriteLine($"\nTotal tipuri abonamente: {abonamente.Count}\n");


            // --- Test 6: Istoric citire ---
            var istoricAccessor = new IstoricCitireAccessor();
            var istorice = istoricAccessor.GetAll();

            Console.WriteLine("Istoric citiri:");
            foreach (var i in istorice)
            {
                Console.WriteLine($"  [{i.id_istoric}] Utilizator {i.id_utilizator} -> Carte {i.id_carte}, acțiune: {i.actiune}, data: {i.data_accesare}");
            }

            Console.WriteLine($"\nTotal intrări istoric: {istorice.Count}\n");


            // --- Test 7: Serii ---
            var serieAccessor = new SerieAccessor();
            var serii = serieAccessor.GetAll();

            Console.WriteLine("Serii de carti:");
            foreach (var s in serii)
            {
                Console.WriteLine($"  [{s.id_serie}] {s.nume_serie} - {s.descriere}");
            }

            Console.WriteLine($"\nTotal serii: {serii.Count}\n");

            Console.WriteLine("=== Testare completă ===");
            Console.WriteLine("\nApasă Enter pentru a închide...");
            Console.ReadLine();
        }
    }
}
