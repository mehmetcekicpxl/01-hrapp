using HrApp.Services.Interfaces;
using HrApp.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HrApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IIdentityService _identityService;
     
        public AccountController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        #region Login
        public IActionResult Login()
        {
            return View();
        }
        #endregion

        #region Login Username

        [HttpGet]
        public IActionResult LoginUserName()
        {
            return View();
        }

        //TODO

        #endregion

        #region Login Email

        [HttpGet]
        public IActionResult LoginEmail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Gebruik Task<IActionResult> omdat het een async methode is
        public async Task<IActionResult> LoginEmail(LoginEmailViewModel loginEmail)
        {
            if (ModelState.IsValid)
            {
                // Roep de service aan en wacht op het resultaat met 'await'
                var result = await _identityService.LoginWithEmailAsync(loginEmail.Email, loginEmail.Password);

                if (result.Succeeded)
                {
                    // Als het inloggen lukt, stuur de gebruiker naar de homepagina
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Voeg de foutmeldingen toe aan de ModelState om ze op het scherm te tonen
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
            }

            // Als we hier zijn gekomen, is er iets mislukt. Toon het formulier opnieuw.
            return View(loginEmail);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Gebruik Task<IActionResult> omdat het een async methode is
        public async Task<IActionResult> LoginUserName(LoginUserNameViewModel loginUserName)
        {
            if (ModelState.IsValid)
            {
                // Roep de service aan en wacht op het resultaat met 'await'
                var result = await _identityService.LoginWithUsernameAsync(loginUserName.UserName, loginUserName.Password);

                if (result.Succeeded)
                {
                    // Als het inloggen lukt, stuur de gebruiker naar de homepagina
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Voeg de foutmeldingen toe aan de ModelState om ze op het scherm te tonen
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
            }

            // Als we hier zijn gekomen, is er iets mislukt. Toon het formulier opnieuw.
            return View(loginUserName);
        }

        //TODO

        #endregion

        #region Register

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel registerModel)
        {
            if (ModelState.IsValid)
            {
                // Roep de service aan om de registratie logica uit te voeren
                var result = await _identityService.RegisterAsync(registerModel);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    // Voeg de foutmeldingen vanuit de service toe aan de ModelState
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
            }
            return View();
        }

        #endregion

        #region Logout

        public async Task<IActionResult> LogoutAsync()
        {
            // 1. Log lokaal uit uit onze eigen applicatie (HrApp)
            await _identityService.SignOutAsync();

            // 2. Stuur een verzoek naar Duende (oidc) om daar ook de sessie te vernietigen
            // Nadat Duende is uitgelogd, sturen we de gebruiker terug naar ons Login scherm
            return SignOut(new AuthenticationProperties
            {
                RedirectUri = Url.Action("Login", "Account")
            }, "oidc");
        }

        #endregion









        public IActionResult LoginExternalProvider()
        {
            string? redirectUrl = Url.Action("ExternalProviderResponse", "Account");
            string scheme = "oidc";
            var properties = _identityService.ConfigureExternalAuthenticationProperties(
                    scheme, redirectUrl);
            return new ChallengeResult(scheme, properties);
        }

        public async Task<IActionResult> ExternalProviderResponse()
        {
            // Laat de IdentityService al het zware werk doen
            var result = await _identityService.HandleExternalLoginAsync();

            if (result.Succeeded)
            {
                // Als alles goed ging, stuur de gebruiker naar de homepagina
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Als er iets mis is gegaan, stuur de gebruiker terug naar het login scherm
                return RedirectToAction(nameof(Login));
            }
        }

        public IActionResult LoginGoogle()
        {
            // 1. Google'dan dönünce nereye geleceğini belirt
            string? redirectUrl = Url.Action("ExternalProviderResponse", "Account");

            // 2. "Google" şemasını kullan (Program.cs'te .AddGoogle() ile tanımladığımız isim)
            string scheme = "Google";

            // 3. ChallengeResult ile Google'a "Bu adamı doğrula ve bana yolla" de
            var properties = _identityService.ConfigureExternalAuthenticationProperties(scheme, redirectUrl);

            return new ChallengeResult(scheme, properties);
        }
    }
}
