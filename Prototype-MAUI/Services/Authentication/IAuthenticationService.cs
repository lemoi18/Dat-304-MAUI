using MauiApp8.Model;

namespace MauiApp8.Services.Authentication
{
    public interface IAuthenticationService
    {


        Account User { get; set; }

        Task<Account> AuthenticateAsync(CancellationToken cancellationToken);
        Task<Account> RefreshAuthenticateAsync();



        Task SignOutAsync();

    }
}
