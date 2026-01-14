using eBooks_MVC.Models;

using LibrarieModele;

using NivelAccessDate;

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
        private readonly CarteAccessor carteAccessor = new CarteAccessor();

        // GET: Carti
        public ActionResult Index()
        {
            var carti = context.Carti.Include("Autor").ToList();
            var model = carti.Select(c => new CarteViewModel
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
                    id_autor = model.id_autor
                };

                context.Carti.Add(carte);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            model.Autori = context.Autori
                .Select(a => new SelectListItem
                {
                    Text = a.nume_autor,
                    Value = a.id_autor.ToString()
                }).ToList();

            return View(model);
        }


        // GET:Autori/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var carte = carteAccessor.GetById(id.Value);
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
        // POST: Autori/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            carteAccessor.Delete(id); // aici poate fi logic sau fizic
            return RedirectToAction("Index");
        }
    }
    
}