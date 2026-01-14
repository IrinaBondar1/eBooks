using eBooks_MVC.Models;
using LibrarieModele;
using NivelAccessDate;
using NivelServicii;
using NivelServicii.Cache;
using Repository_CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace eBooks_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICarteService carteService;
        private readonly IAutorService autorService;
        private readonly CategorieAccessor categorieAccessor;
        private readonly UtilizatorAccessor utilizatorAccessor;
        private readonly IstoricCitireAccessor istoricAccessor;
        private readonly eBooksContext context;

        // Constructor pentru Dependency Injection
        public HomeController(
            ICarteService carteService,
            IAutorService autorService,
            CategorieAccessor categorieAccessor,
            UtilizatorAccessor utilizatorAccessor,
            IstoricCitireAccessor istoricAccessor,
            eBooksContext context)
        {
            this.carteService = carteService;
            this.autorService = autorService;
            this.categorieAccessor = categorieAccessor;
            this.utilizatorAccessor = utilizatorAccessor;
            this.istoricAccessor = istoricAccessor;
            this.context = context;
        }

        // Fallback constructor pentru compatibilitate (dacă DI nu este configurat)
        public HomeController() : this(
            new CarteService(new CarteAccessor(), new MemoryCacheService()),
            new AutorService(new AutorAccessor(), new MemoryCacheService()),
            new CategorieAccessor(),
            new UtilizatorAccessor(),
            new IstoricCitireAccessor(),
            new eBooksContext())
        {
        }

        // GET: Home/Index - Pagina principală cu statistici
        [AllowAnonymous]
        public ActionResult Index()
        {
            // Logica de business folosind servicii
            var statistici = new HomeStatisticsViewModel
            {
                // Statistici cărți (folosind CarteService cu cache)
                TotalCarti = carteService.GetAll().Count,
                CartiRecente = carteService.GetAll()
                    .OrderByDescending(c => c.id_carte)
                    .Take(5)
                    .ToList(),

                // Statistici autori (folosind AutorService cu cache)
                TotalAutori = autorService.GetAll().Count,
                AutoriPopulari = autorService.GetAll()
                    .Take(5)
                    .ToList(),

                // Statistici categorii
                TotalCategorii = categorieAccessor.GetAll().Count,
                Categorii = categorieAccessor.GetAll()
                    .Take(6)
                    .ToList(),

                // Statistici utilizatori
                TotalUtilizatori = utilizatorAccessor.GetAll().Count,
                UtilizatoriNoi = utilizatorAccessor.GetAll()
                    .OrderByDescending(u => u.id_utilizator)
                    .Take(5)
                    .ToList(),

                // Statistici citiri (folosind accessor pentru istoric)
                TotalCitiri = istoricAccessor.GetAll().Count,
                CitiriRecente = istoricAccessor.GetAll()
                    .OrderByDescending(i => i.data_accesare)
                    .Take(10)
                    .ToList(),

                // Cărți populare (bazat pe numărul de citiri)
                CartiPopulare = GetCartiPopulare()
            };

            return View(statistici);
        }

        // Metodă helper pentru a calcula cărțile populare
        private List<Carte> GetCartiPopulare()
        {
            var citiri = istoricAccessor.GetAll();
            var cartiIds = citiri
                .GroupBy(c => c.id_carte)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .Select(g => g.Key)
                .ToList();

            var cartiPopulare = new List<Carte>();
            foreach (var carteId in cartiIds)
            {
                var carte = carteService.GetById(carteId);
                if (carte != null)
                {
                    cartiPopulare.Add(carte);
                }
            }

            return cartiPopulare;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }

    // ViewModel pentru statistici
    public class HomeStatisticsViewModel
    {
        public int TotalCarti { get; set; }
        public List<Carte> CartiRecente { get; set; }
        public int TotalAutori { get; set; }
        public List<Autor> AutoriPopulari { get; set; }
        public int TotalCategorii { get; set; }
        public List<Categorie> Categorii { get; set; }
        public int TotalUtilizatori { get; set; }
        public List<Utilizator> UtilizatoriNoi { get; set; }
        public int TotalCitiri { get; set; }
        public List<IstoricCitire> CitiriRecente { get; set; }
        public List<Carte> CartiPopulare { get; set; }
    }
}
