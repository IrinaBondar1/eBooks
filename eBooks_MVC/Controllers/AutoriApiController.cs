using eBooks_MVC.Models.Api;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Formatting;


namespace eBooks_MVC.Controllers
{
    public class AutoriApiController : Controller
    {
        private readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("http://proiect-api-demo.com/api/")
        };

        public async Task<ActionResult> Index()
        {
            var response = await client.GetAsync("autori");
            if (!response.IsSuccessStatusCode) return View(new List<AutorApiModel>());
            var autori = await response.Content.ReadAsAsync<List<AutorApiModel>>();
            return View(autori);
        }

        public ActionResult Create() => View();

        [HttpPost]
        public async Task<ActionResult> Create(AutorApiModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var response = await client.PostAsJsonAsync("autori", model);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int id)
        {
            var response = await client.GetAsync($"autori/{id}");
            var autor = await response.Content.ReadAsAsync<AutorApiModel>();
            return View(autor);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(AutorApiModel model)
        {
            var response = await client.PutAsJsonAsync($"autori/{model.id_autor}", model);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(int id)
        {
            var response = await client.GetAsync($"autori/{id}");
            var autor = await response.Content.ReadAsAsync<AutorApiModel>();
            return View(autor);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await client.DeleteAsync($"autori/{id}");
            return RedirectToAction("Index");
        }
    }


}