using eBooks_MVC.Models;
using LibrarieModele;
using NivelAccessDate;
using NivelServicii;
using Repository_CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eBooks_MVC.Controllers
{
    [Authorize]
    public class UtilizatorController : Controller
    {
        private readonly TipAbonamentAccessor tipAbonamentAccessor = new TipAbonamentAccessor();
        private readonly ICarteService carteService;
        private readonly IAutorService autorService;
        private readonly UtilizatorAccessor utilizatorAccessor = new UtilizatorAccessor();
        private readonly IstoricCitireAccessor istoricAccessor = new IstoricCitireAccessor();
        private readonly AccesService accesService;
        private readonly eBooksContext context = new eBooksContext();

        public UtilizatorController()
        {
            // Inițializare servicii cu cache
            var carteAccessor = new CarteAccessor();
            var autorAccessor = new AutorAccessor();
            var cache = new NivelServicii.Cache.MemoryCacheService();
            
            carteService = new CarteService(carteAccessor, cache);
            autorService = new AutorService(autorAccessor, cache);
            accesService = new AccesService(utilizatorAccessor, istoricAccessor);
        }

        private Utilizator GetCurrentUser()
        {
            if (Session["UtilizatorId"] == null)
                return null;

            int userId = (int)Session["UtilizatorId"];
            var user = utilizatorAccessor.GetById(userId);
            
            // Actualizeaza session daca user-ul exista
            if (user != null && user.TipAbonament != null)
            {
                Session["TipAbonament"] = user.TipAbonament.denumire;
            }
            
            return user;
        }

        // GET: Utilizator/Index - Afiseaza planurile de abonament
        [AllowAnonymous]
        public ActionResult Index()
        {
            var planuri = tipAbonamentAccessor.GetAll();
            var currentUser = GetCurrentUser();
            
            var model = planuri.Select(p => new TipAbonamentViewModel
            {
                id_tip_abonament = p.id_tip_abonament,
                denumire = p.denumire,
                limita_carti_pe_luna = p.limita_carti_pe_luna == int.MaxValue ? -1 : p.limita_carti_pe_luna, // -1 = nelimitat
                acces_serii_complete = p.acces_serii_complete,
                permite_descarcare = p.permite_descarcare
            }).ToList();

            ViewBag.CurrentPlanId = currentUser?.id_tip_abonament ?? 1;
            return View(model);
        }

        // GET: Utilizator/Carti - Afiseaza cartile disponibile (cu filtrare bazata pe abonament)
        public ActionResult Carti()
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            // Folosim serviciile pentru a obține cărțile și autorii (cu cache)
            var carti = carteService.GetAll();
            var autori = autorService.GetAll();
            
            // Filtreaza cartile bazat pe tipul de abonament
            var cartiFiltrate = new List<Carte>();
            foreach (var carte in carti)
            {
                if (accesService.PoateAccesaCarte(currentUser, carte))
                {
                    cartiFiltrate.Add(carte);
                }
            }

            var model = cartiFiltrate.Select(c =>
            {
                var autor = autori.FirstOrDefault(a => a.id_autor == c.id_autor);
                var carteContext = context.Carti
                    .Include("Serie")
                    .Include("Categorie")
                    .FirstOrDefault(ca => ca.id_carte == c.id_carte);

                return new CarteViewModel
                {
                    id_carte = c.id_carte,
                    titlu = c.titlu,
                    descriere = c.descriere,
                    id_autor = c.id_autor,
                    nume_autor = autor != null ? autor.nume_autor : "N/A",
                    nume_serie = carteContext?.Serie != null ? carteContext.Serie.nume_serie : null,
                    nr_volum = carteContext?.nr_volum
                };
            }).ToList();

            ViewBag.CanDownload = accesService.PoateDescarca(currentUser);
            ViewBag.CanReadMore = accesService.PoateCitireInca(currentUser);
            ViewBag.CurrentUser = currentUser;

            return View(model);
        }

        // GET: Utilizator/DetaliiCarte/5 - Detalii carte cu verificare acces
        public ActionResult DetaliiCarte(int? id)
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var carte = context.Carti
                .Include("Autor")
                .Include("Serie")
                .Include("Categorie")
                .FirstOrDefault(c => c.id_carte == id.Value);

            if (carte == null || carte.IsDeleted)
                return HttpNotFound();

            // Verifica accesul
            if (!accesService.PoateAccesaCarte(currentUser, carte))
            {
                TempData["Error"] = "Nu aveti acces la aceasta carte cu planul dvs. actual. Upgrade la Premium sau VIP pentru acces complet!";
                return RedirectToAction("Carti");
            }

            var model = new CarteViewModel
            {
                id_carte = carte.id_carte,
                titlu = carte.titlu,
                descriere = carte.descriere,
                id_autor = carte.id_autor,
                nume_autor = carte.Autor != null ? carte.Autor.nume_autor : "N/A",
                nume_serie = carte.Serie != null ? carte.Serie.nume_serie : null,
                nr_volum = carte.nr_volum,
                nume_categorie = carte.Categorie != null ? carte.Categorie.denumire : null
            };

            ViewBag.CanDownload = accesService.PoateDescarca(currentUser);
            ViewBag.CurrentUser = currentUser;

            return View(model);
        }

        // POST: Utilizator/CitesteCarte/5 - Incearca sa citeasca o carte
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CitesteCarte(int id)
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            // Folosim serviciul pentru a obține cartea (cu cache)
            var carte = carteService.GetById(id);
            if (carte == null)
                return HttpNotFound();

            if (!accesService.PoateAccesaCarte(currentUser, carte))
            {
                TempData["Error"] = "Nu mai puteti citi carti in aceasta luna sau nu aveti acces la aceasta carte!";
                return RedirectToAction("Carti");
            }

            // Inregistreaza accesul
            accesService.InregistrareAccesCarte(currentUser, carte, "citire");

            TempData["Success"] = $"Ati inceput sa cititi: {carte.titlu}";
            return RedirectToAction("DetaliiCarte", new { id = id });
        }

        // POST: Utilizator/DownloadCarte/5 - Descarca carte (doar VIP)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DownloadCarte(int id)
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            if (!accesService.PoateDescarca(currentUser))
            {
                TempData["Error"] = "Descarcarea este disponibila doar pentru planul VIP! Upgrade acum!";
                return RedirectToAction("DetaliiCarte", new { id = id });
            }

            // Folosim serviciul pentru a obține cartea (cu cache)
            var carte = carteService.GetById(id);
            if (carte == null)
                return HttpNotFound();

            // Inregistreaza descarcarea
            accesService.InregistrareAccesCarte(currentUser, carte, "descarcare");

            TempData["Success"] = $"Descarcare initiata pentru: {carte.titlu} (simulare)";
            return RedirectToAction("DetaliiCarte", new { id = id });
        }

        // GET: Utilizator/Istoric - Istoric citire pentru utilizatorul curent
        public ActionResult Istoric()
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            var istoric = istoricAccessor.GetByUtilizatorId(currentUser.id_utilizator);

            var model = istoric.Select(i => new IstoricCitireViewModel
            {
                id_istoric = i.id_istoric,
                titlu_carte = i.Carte != null ? i.Carte.titlu : "N/A",
                data_accesare = i.data_accesare,
                actiune = i.actiune ?? "N/A"
            }).ToList();

            ViewBag.UtilizatorId = currentUser.id_utilizator;
            return View(model);
        }

        // GET: Utilizator/Profil - Profil utilizator curent
        public ActionResult Profil()
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            var utilizator = utilizatorAccessor.GetById(currentUser.id_utilizator);
            if (utilizator == null)
                return HttpNotFound();

            var plan = tipAbonamentAccessor.GetById(utilizator.id_tip_abonament);
            var model = new UtilizatorViewModel
            {
                id_utilizator = utilizator.id_utilizator,
                nume_utilizator = utilizator.nume_utilizator,
                email = utilizator.email,
                data_inregistrare = utilizator.data_inregistrare,
                denumire_plan = plan != null ? plan.denumire : "N/A",
                carti_citite_luna = utilizator.carti_citite_luna,
                limita_carti_pe_luna = plan != null ? (plan.limita_carti_pe_luna == int.MaxValue ? -1 : plan.limita_carti_pe_luna) : 0
            };

            // Obtine planurile disponibile pentru upgrade
            var allPlans = tipAbonamentAccessor.GetAll();
            ViewBag.AvailablePlans = allPlans
                .Where(p => p.id_tip_abonament > utilizator.id_tip_abonament)
                .Select(p => new SelectListItem
                {
                    Text = $"{p.denumire} - {(p.limita_carti_pe_luna == int.MaxValue ? "Nelimitat" : p.limita_carti_pe_luna.ToString())} carti/luna",
                    Value = p.id_tip_abonament.ToString()
                }).ToList();

            return View(model);
        }

        // POST: Utilizator/UpgradeCont - Upgrade contul utilizatorului
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpgradeCont(int? idTipAbonamentNou)
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            var utilizator = utilizatorAccessor.GetById(currentUser.id_utilizator);
            if (utilizator == null)
                return HttpNotFound();

            // Verifica ca un plan a fost selectat
            if (!idTipAbonamentNou.HasValue || idTipAbonamentNou.Value <= 0)
            {
                TempData["Error"] = "Va rugam selectati un plan pentru upgrade!";
                return RedirectToAction("Profil");
            }

            // Verifica ca upgrade-ul este valid (Free -> Premium -> VIP)
            if (idTipAbonamentNou.Value <= utilizator.id_tip_abonament)
            {
                TempData["Error"] = "Nu puteti face downgrade! Selectati un plan superior planului curent.";
                return RedirectToAction("Profil");
            }

            var newPlan = tipAbonamentAccessor.GetById(idTipAbonamentNou.Value);
            if (newPlan == null)
            {
                TempData["Error"] = "Planul selectat nu exista!";
                return RedirectToAction("Profil");
            }

            // Actualizeaza planul direct in context pentru a evita problemele cu relatii
            using (var ctx = new eBooksContext())
            {
                var utilizatorDb = ctx.Utilizatori.Find(currentUser.id_utilizator);
                if (utilizatorDb == null)
                {
                    TempData["Error"] = "Utilizatorul nu a fost gasit!";
                    return RedirectToAction("Profil");
                }

                utilizatorDb.id_tip_abonament = idTipAbonamentNou.Value;
                ctx.SaveChanges();
            }

            // Actualizeaza session
            Session["TipAbonament"] = newPlan.denumire;

            TempData["Success"] = $"Contul dvs. a fost upgrade-uit la {newPlan.denumire}!";
            return RedirectToAction("Profil");
        }
    }
}
