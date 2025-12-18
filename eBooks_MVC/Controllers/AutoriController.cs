using eBooks_MVC.Models;
using LibrarieModele;

using NivelAccessDate;

using NivelServicii;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace eBooks_MVC.Controllers
{
    public class AutoriController : Controller
    {
        private readonly IAutorService autorService;

        // GET: Autori
        public ActionResult Index()
        {
            var autori = autorService.GetAll();
            var model = autori.Select(a => new AutorViewModel
            {
                id_autor = a.id_autor,
                nume_autor = a.nume_autor,
                data_nasterii = a.data_nasterii,
                tara_origine = a.tara_origine,
                activ = true
            }).ToList();

            return View(model);
        }

        // GET: Autori/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var autor = autorService.GetById(id.Value);
            if (autor == null)
                return HttpNotFound();

            var model = new AutorViewModel
            {
                id_autor = autor.id_autor,
                nume_autor = autor.nume_autor,
                data_nasterii = autor.data_nasterii,
                tara_origine = autor.tara_origine,
                activ = true
            };

            return View(model);
        }
        // ==============================
        // CREATE
        // ==============================

        // GET: Autori/Create
        public ActionResult Create()
        {
            return View(new AutorViewModel());
        }

        // POST: Autori/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AutorViewModel model)
        {
            if (ModelState.IsValid)
            {
                var autor = new LibrarieModele.Autor
                {
                    nume_autor = model.nume_autor,
                    data_nasterii = model.data_nasterii,
                    tara_origine = model.tara_origine
                };

                autorService.Add(autor);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // ==============================
        // EDIT
        // ==============================

        // GET: Autori/Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var autor = autorService.GetById(id.Value);
            if (autor == null)
                return HttpNotFound();

            var model = new AutorViewModel
            {
                id_autor = autor.id_autor,
                nume_autor = autor.nume_autor,
                data_nasterii = autor.data_nasterii,
                tara_origine = autor.tara_origine
            };

            return View(model);
        }

        // POST: Autori/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AutorViewModel model)
        {
            if (ModelState.IsValid)
            {
                var autor = new LibrarieModele.Autor
                {
                    id_autor = model.id_autor,
                    nume_autor = model.nume_autor,
                    data_nasterii = model.data_nasterii,
                    tara_origine = model.tara_origine
                };

                autorService.Update(autor);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Autori/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var autor = autorService.GetById(id.Value);
            if (autor == null)
                return HttpNotFound();

            var model = new AutorViewModel
            {
                id_autor = autor.id_autor,
                nume_autor = autor.nume_autor,
                data_nasterii = autor.data_nasterii,
                tara_origine = autor.tara_origine
            };

            return View(model);
        }

        // POST: Autori/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            autorService.Delete(id); 
            return RedirectToAction("Index");
        }


    }
}