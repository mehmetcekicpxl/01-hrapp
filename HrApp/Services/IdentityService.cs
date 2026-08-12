using HrApp.Services.Interfaces;
using HrApp.Services.Result;
using HrApp.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace HrApp.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public IdentityService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityServiceResult> LoginWithEmailAsync(string email, string password)
        {
            var serviceResult = new IdentityServiceResult();

            // Zoek eerst de gebruiker op basis van e-mailadres
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                serviceResult.Succeeded = false;
                serviceResult.AddError("Ongeldig e-mailadres of wachtwoord.");
                return serviceResult;
            }

            // Log in met de gevonden gebruikersnaam
            var result = await _signInManager.PasswordSignInAsync(user.UserName, password, false, false);

            if (result.Succeeded)
            {
                serviceResult.Succeeded = true;
            }
            else
            {
                serviceResult.Succeeded = false;
                serviceResult.AddError("Ongeldig e-mailadres of wachtwoord.");
            }

            return serviceResult;
        }

        public async Task<IdentityServiceResult> LoginWithUsernameAsync(string username, string password)
        {
            var serviceResult = new IdentityServiceResult();

            // Log direct in met de gebruikersnaam
            var result = await _signInManager.PasswordSignInAsync(username, password, false, false);

            if (result.Succeeded)
            {
                serviceResult.Succeeded = true;
            }
            else
            {
                serviceResult.Succeeded = false;
                serviceResult.AddError("Ongeldige gebruikersnaam of wachtwoord.");
            }

            return serviceResult;
        }
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();

        }

        public async Task<IdentityServiceResult> RegisterAsync(RegisterViewModel registerData)
        {
            // Maak een nieuw IdentityUser object aan met de ingevoerde gegevens
            var identityUser = new IdentityUser
            {
                UserName = registerData.UserName,
                Email = registerData.Email
            };
            // Probeer de nieuwe gebruiker in de database op te slaan
            var result = await _userManager.CreateAsync(identityUser, registerData.Password);


            // Maak ons eigen resultaat object aan, stel alleen Succeeded in
            var serviceResult = new IdentityServiceResult
            {
                Succeeded = result.Succeeded,

            };
            // Als het mislukt is, gebruik de AddError methode om fouten één voor één toe te voegen
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    serviceResult.AddError(error.Description);
                }
            }
            return serviceResult;
        }





        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl)
        {
            return _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        }

        // 1. De hoofd methode die door de controller wordt aangeroepen
        public async Task<IdentityServiceResult> HandleExternalLoginAsync()
        {
            var serviceResult = new IdentityServiceResult();

            // Haal de login informatie op van de externe provider (bijv. Duende)
            ExternalLoginInfo? externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();

            if (externalLoginInfo == null)
            {
                serviceResult.Succeeded = false;
                serviceResult.AddError("Kan externe login informatie niet ophalen.");
                return serviceResult;
            }

            // Zoek of de gebruiker al bestaat in onze database via de externe provider key
            var user = await _userManager.FindByLoginAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey);

            if (user == null)
            {
                // Als de gebruiker niet bestaat, probeer een nieuwe aan te maken via claims
                user = await CreateIdentityUserFromClaims(externalLoginInfo);

                if (user == null)
                {
                    serviceResult.Succeeded = false;
                    serviceResult.AddError("Fout bij het aanmaken van een gebruiker via de externe provider.");
                    return serviceResult;
                }
            }

            // Log de gebruiker succesvol in
            await _signInManager.SignInAsync(user, isPersistent: true);
            serviceResult.Succeeded = true;

            return serviceResult;
        }

        // 2. De private helper methode (alleen zichtbaar binnen deze service)
        private async Task<IdentityUser?> CreateIdentityUserFromClaims(ExternalLoginInfo externalLoginInfo)
        {
            // Zoek naar het e-mailadres in de claims van de externe provider
            var claim = externalLoginInfo.Principal.FindFirst(ClaimTypes.Email) ?? externalLoginInfo.Principal.FindFirst("email");

            if (claim != null)
            {
                var email = claim.Value;

                // Controleer of er al een gebruiker is met dit e-mailadres
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    // Maak een nieuwe IdentityUser aan
                    user = new IdentityUser { UserName = email, Email = email };
                    var result = await _userManager.CreateAsync(user);

                    if (!result.Succeeded)
                    {
                        return null; // Stop als het aanmaken mislukt
                    }
                }

                // Koppel de externe login (bijv. Duende) aan deze gebruiker
                var loginResult = await _userManager.AddLoginAsync(user, externalLoginInfo);

                if (loginResult.Succeeded)
                {
                    return user;
                }
            }
            return null;
        }
    }
}
