using HrApp.Services.Result;
using HrApp.ViewModels;
using Microsoft.AspNetCore.Authentication;

namespace HrApp.Services.Interfaces
{
    public interface IIdentityService
    {
        // Methode voor inloggen met e-mailadres
        Task<IdentityServiceResult> LoginWithEmailAsync(string email, string password);

        // Methode voor inloggen met gebruikersnaam
        Task<IdentityServiceResult> LoginWithUsernameAsync(string username, string password);
        Task SignOutAsync();
        Task<IdentityServiceResult> RegisterAsync(RegisterViewModel registerData);

        // Nieuwe methoden voor externe login
        AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl);
        Task<IdentityServiceResult> HandleExternalLoginAsync();
    }
}
