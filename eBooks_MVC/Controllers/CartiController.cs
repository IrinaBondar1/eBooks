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
using System.Web;
using System.Web.Mvc;

namespace eBooks_MVC.Controllers
{
    public class CartiController : Controller
    {
        private readonly eBooksContext context = new eBooksContext();
        private readonly ICarteService carteService;

        public CartiController()
        {
            var carteAccessor = new CarteAccessor();
            var cache = new MemoryCacheService();
            carteService = new CarteService(carteAccessor, cache);
        }

        // GET: Carti
        public ActionResult Index()
        {
            // Obținem direct din context cu Include-uri pentru detalii complete
            // Nu folosim cache-ul aici pentru a evita problemele de conversie SQL
            var cartiCuDetalii = context.Carti
                .Include("Autor")
                .Where(c => !c.IsDeleted)
                .ToList();

            var model = cartiCuDetalii.Select(c => new CarteViewModel
            {
                id_carte = c.id_carte,
                titlu = c.titlu,
                descriere = c.descriere,
                id_autor = c.id_autor,
                nume_autor = c.Autor != null ? c.Autor.nume_autor : "N/A"
            }).ToList();

            return View(model);
        }


        // ==============================
        // CREATE
        // ==============================

        // GET: Carti/Create
        public ActionResult Create()
        {
            return View(new CarteViewModel(context));
        }


        //POST: Carti/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CarteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var carte = new Carte
                {
                    titlu = model.titlu,
                    descriere = model.descriere,
                    id_autor = model.id_autor,
                    IsDeleted = false
                };

                // Folosim serviciul pentru adăugare (cu invalidare cache)
                carteService.Add(carte);
                return RedirectToAction("Index");
            }

            model.Autori = context.Autori
                .Where(a => !a.IsDeleted)
                .Select(a => new SelectListItem
                {
                    Text = a.nume_autor,
                    Value = a.id_autor.ToString()
                }).ToList();

            return View(model);
        }


        // GET: Carti/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            // Folosim serviciul pentru a obține cartea (cu cache)
            var carte = carteService.GetById(id.Value);
            if (carte == null)
                return HttpNotFound();

            var model = new CarteViewModel
            {
                id_carte = carte.id_carte,
                titlu = carte.titlu,
                descriere = carte.descriere
            };

            return View(model);
        }
        // POST: Carti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Folosim serviciul pentru ștergere logică (cu invalidare cache)
            carteService.SoftDelete(id);
            return RedirectToAction("Index");
        }
    }
    
}