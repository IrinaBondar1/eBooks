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
using System.Net;
using System.Web.Mvc;

namespace eBooks_MVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly CategorieAccessor categorieAccessor = new CategorieAccessor();
        private readonly ICarteService carteService;
        private readonly AutorAccessor autorAccessor = new AutorAccessor();
        private readonly SerieAccessor serieAccessor = new SerieAccessor();
        private readonly eBooksContext context = new eBooksContext();

        public AdminController()
        {
            var carteAccessor = new CarteAccessor();
            var cache = new MemoryCacheService();
            carteService = new CarteService(carteAccessor, cache);
        }

        // ==============================
        // CATEGORIE - CRUD Operations
        // ==============================

        // GET: Admin/Categorie
        public ActionResult Categorie()
        {
            var categorii = categorieAccessor.GetAll();
            var model = categorii.Select(c => new CategorieViewModel
            {
                id_categorie = c.id_categorie,
                denumire = c.denumire,
                descriere = c.descriere
            }).ToList();

            return View(model);
        }

        // GET: Admin/Categorie/Create
        public ActionResult CreateCategorie()
        {
            return View(new CategorieViewModel());
        }

        // POST: Admin/Categorie/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCategorie(CategorieViewModel model)
        {
            if (ModelState.IsValid)
            {
                var categorie = new Categorie
                {
                    denumire = model.denumire,
                    descriere = model.descriere,
                    IsDeleted = false
                };

                categorieAccessor.Add(categorie);
                return RedirectToAction("Categorie");
            }

            return View(model);
        }

        // GET: Admin/Categorie/Edit
        public ActionResult EditCategorie(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var categorie = categorieAccessor.GetById(id.Value);
            if (categorie == null)
                return HttpNotFound();

            var model = new CategorieViewModel
            {
                id_categorie = categorie.id_categorie,
                denumire = categorie.denumire,
                descriere = categorie.descriere
            };

            return View(model);
        }

        // POST: Admin/Categorie/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategorie(CategorieViewModel model)
        {
            if (ModelState.IsValid)
            {
                var categorie = categorieAccessor.GetById(model.id_categorie);
                if (categorie == null)
                    return HttpNotFound();

                categorie.denumire = model.denumire;
                categorie.descriere = model.descriere;

                categorieAccessor.Update(categorie);
                return RedirectToAction("Categorie");
            }

            return View(model);
        }

        // GET: Admin/Categorie/Delete/
        public ActionResult DeleteCategorie(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var categorie = categorieAccessor.GetById(id.Value);
            if (categorie == null)
                return HttpNotFound();

            var model = new CategorieViewModel
            {
                id_categorie = categorie.id_categorie,
                denumire = categorie.denumire,
                descriere = categorie.descriere
            };

            return View(model);
        }

        // POST: Admin/Categorie/Delete/
        [HttpPost, ActionName("DeleteCategorie")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCategorieConfirmed(int id)
        {
            // Stergere logica
            categorieAccessor.SoftDelete(id);
            return RedirectToAction("Categorie");
        }

        // ==============================
        // CARTE - CRUD Operations
        // ==============================

        // GET: Admin/Carte
        public ActionResult Carte()
        {
            
            var cartiCuDetalii = context.Carti
                .Include("Autor")
                .Include("Categorie")
                .Include("Serie")
                .Where(c => !c.IsDeleted)
                .ToList();

            var model = cartiCuDetalii.Select(c => new CarteViewModel
            {
                id_carte = c.id_carte,
                titlu = c.titlu,
                descriere = c.descriere,
                id_autor = c.id_autor,
                nume_autor = c.Autor != null ? c.Autor.nume_autor : "N/A",
                nume_categorie = c.Categorie != null ? c.Categorie.denumire : null,
                nume_serie = c.Serie != null ? c.Serie.nume_serie : null,
                nr_volum = c.nr_volum
            }).ToList();

            return View(model);
        }

        // GET: Admin/Carte/Create
        public ActionResult CreateCarte()
        {
            return View(new CarteViewModel(context));
        }

        // POST: Admin/Carte/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCarte(CarteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var carte = new Carte
                {
                    titlu = model.titlu,
                    descriere = model.descriere,
                    id_autor = model.id_autor,
                    id_categorie = model.id_categorie,
                    id_serie = model.id_serie,
                    nr_volum = model.nr_volum,
                    IsDeleted = false
                };

                carteService.Add(carte);
                return RedirectToAction("Carte");
            }

            // Re-populate dropdowns in caz de eroare
            var viewModel = new CarteViewModel(context)
            {
                titlu = model.titlu,
                descriere = model.descriere,
                id_autor = model.id_autor,
                id_categorie = model.id_categorie,
                id_serie = model.id_serie,
                nr_volum = model.nr_volum
            };

            return View(viewModel);
        }

        // GET: Admin/Carte/Edit/5
        public ActionResult EditCarte(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var carte = context.Carti
                .Include("Autor")
                .Include("Categorie")
                .Include("Serie")
                .FirstOrDefault(c => c.id_carte == id.Value);

            if (carte == null || carte.IsDeleted)
                return HttpNotFound();

            var model = new CarteViewModel(context)
            {
                id_carte = carte.id_carte,
                titlu = carte.titlu,
                descriere = carte.descriere,
                id_autor = carte.id_autor,
                id_categorie = carte.id_categorie,
                id_serie = carte.id_serie,
                nr_volum = carte.nr_volum
            };

            return View(model);
        }

        // POST: Admin/Carte/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCarte(CarteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var carte = carteService.GetById(model.id_carte);
                if (carte == null || carte.IsDeleted)
                    return HttpNotFound();

                carte.titlu = model.titlu;
                carte.descriere = model.descriere;
                carte.id_autor = model.id_autor;
                carte.id_categorie = model.id_categorie;
                carte.id_serie = model.id_serie;
                carte.nr_volum = model.nr_volum;

                carteService.Update(carte);
                return RedirectToAction("Carte");
            }

            // Re-populate dropdowns in caz de eroare
            var viewModel = new CarteViewModel(context)
            {
                id_carte = model.id_carte,
                titlu = model.titlu,
                descriere = model.descriere,
                id_autor = model.id_autor,
                id_categorie = model.id_categorie,
                id_serie = model.id_serie,
                nr_volum = model.nr_volum
            };

            return View(viewModel);
        }

        // GET: Admin/Carte/Delete/5
        public ActionResult DeleteCarte(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var carte = carteService.GetById(id.Value);
            if (carte == null)
                return HttpNotFound();

            var autor = autorAccessor.GetById(carte.id_autor);
            var model = new CarteViewModel
            {
                id_carte = carte.id_carte,
                titlu = carte.titlu,
                descriere = carte.descriere,
                id_autor = carte.id_autor,
                nume_autor = autor != null ? autor.nume_autor : "N/A"
            };

            return View(model);
        }

        // POST: Admin/Carte/Delete/5
        [HttpPost, ActionName("DeleteCarte")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCarteConfirmed(int id)
        {
            // Stergere logica 
            carteService.SoftDelete(id);
            return RedirectToAction("Carte");
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
    }
}

