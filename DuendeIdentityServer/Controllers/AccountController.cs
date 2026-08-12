using Duende.IdentityServer;
using Duende.IdentityServer.Services;
using DuendeIdentityServer.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DuendeIdentityServer.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var model = new LoginViewModel();
            model.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // 1. Kullanıcıyı bul VE şifreyi kontrol et
            var testUser = Config.TestUsers.FirstOrDefault(x =>
                x.Username.ToLower() == model.Username.ToLower() &&
                x.Password == model.Password); // Şifre kontrolünü ekledik!

            if (testUser != null)
            {
                // 2. Kullanıcının kimliğini oluştur
                var claims = testUser.Claims;
                var identity = new ClaimsIdentity(claims, "idsrv"); // "idsrv" şemasını kullanıyoruz
                var principal = new ClaimsPrincipal(identity);

                // 3. Giriş yap
                await HttpContext.SignInAsync("idsrv", principal);

                // 4. Güvenli yönlendirme (ReturnUrl boşsa ana sayfaya at)
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                return Redirect("~/");
            }

            // 5. Hata mesajı
            ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            // 1. Verwijder de "idsrv" cookie (Log de gebruiker uit bij Duende)
            await HttpContext.SignOutAsync("idsrv");

            // 2. Stuur de gebruiker veilig terug naar de MVC applicatie (HrApp)
            // Let op: Controleer of 5002 de juiste poort is van jouw HrApp!
            return Redirect("https://localhost:5002");
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            // Maak een nieuw model aan en geef de returnUrl door
            var model = new RegisterViewModel { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            // 1. Controleer of de verplichte velden zijn ingevuld
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError(string.Empty, "Gebruikersnaam en wachtwoord zijn verplicht.");
                return View(model);
            }

            // 2. Controleer of de gebruiker al bestaat in de TestUsers lijst
            var exists = Config.TestUsers.Any(x => x.Username.ToLower() == model.Username.ToLower());
            if (exists)
            {
                ModelState.AddModelError(string.Empty, "Deze gebruikersnaam bestaat al.");
                return View(model);
            }

            // 3. Genereer een nieuw uniek ID voor de TestUser (bijv. 2, 3, 4...)
            var newSubjectId = (Config.TestUsers.Count + 1).ToString();

            // 4. Maak de nieuwe TestUser aan
            var newUser = new Duende.IdentityServer.Test.TestUser
            {
                SubjectId = newSubjectId,
                Username = model.Username,
                Password = model.Password,
                Claims =
        {
            new System.Security.Claims.Claim("sub", newSubjectId),
            new System.Security.Claims.Claim("email", model.Email ?? ""),
            new System.Security.Claims.Claim("name", model.Username)
        }
            };

            // 5. Voeg de nieuwe gebruiker toe aan de lijst in het geheugen (RAM)
            Config.TestUsers.Add(newUser);

            // 6. Stuur de gebruiker na succesvolle registratie door naar het inlogscherm
            return RedirectToAction("Login", new { returnUrl = model.ReturnUrl });
        }
    }
}
