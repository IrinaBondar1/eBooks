using eBooks_MVC.Models;
using LibrarieModele;
using NivelAccessDate;
using Repository_CodeFirst;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace eBooks_MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UtilizatorAccessor utilizatorAccessor = new UtilizatorAccessor();
        private readonly TipAbonamentAccessor tipAbonamentAccessor = new TipAbonamentAccessor();
        private readonly eBooksContext context = new eBooksContext();

        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var utilizator = utilizatorAccessor.GetByEmailAndPassword(model.Email, model.Parola);
                if (utilizator != null)
                {
                    // Creeaza cookie de autentificare
                    FormsAuthentication.SetAuthCookie(utilizator.id_utilizator.ToString(), model.RememberMe);
                    
                    // Salveaza utilizatorul in session
                    Session["UtilizatorId"] = utilizator.id_utilizator;
                    Session["UtilizatorNume"] = utilizator.nume_utilizator;
                    Session["TipAbonament"] = utilizator.TipAbonament?.denumire ?? "Free";

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    // După login, redirecționează către pagina principală
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Email sau parola incorecta.");
                }
            }

            return View(model);
        }

        // GET: Account/Register
        public ActionResult Register()
        {
            // Toti utilizatorii noi incep cu planul Free
            return View(new RegisterViewModel { IdTipAbonament = 1 });
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            // Forteaza valoarea la Free (1) daca nu este setata sau este 0
            if (model.IdTipAbonament <= 0)
            {
                model.IdTipAbonament = 1; // Free
            }

            if (ModelState.IsValid)
            {
                // Verifica daca email-ul exista deja
                if (utilizatorAccessor.GetByEmail(model.Email) != null)
                {
                    ModelState.AddModelError("Email", "Un cont cu acest email exista deja.");
                    return View(model);
                }

                // Forteaza Free pentru toti utilizatorii noi (securitate)
                var utilizator = new Utilizator
                {
                    nume_utilizator = model.NumeUtilizator,
                    email = model.Email,
                    parola = model.Parola, // In productie, ar trebui hash-uita
                    data_inregistrare = DateTime.Now,
                    id_tip_abonament = 1, // Intotdeauna Free pentru utilizatori noi
                    carti_citite_luna = 0
                };

                utilizatorAccessor.Add(utilizator);

                // Auto-login dupa inregistrare
                FormsAuthentication.SetAuthCookie(utilizator.id_utilizator.ToString(), false);
                Session["UtilizatorId"] = utilizator.id_utilizator;
                Session["UtilizatorNume"] = utilizator.nume_utilizator;
                Session["TipAbonament"] = "Free";

                // După înregistrare, redirecționează către pagina principală
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Utilizator");
        }
    }
}
