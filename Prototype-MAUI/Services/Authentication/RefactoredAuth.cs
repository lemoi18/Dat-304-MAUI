using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp8.Model;
using System.Text.Json;


namespace MauiApp8.Services.Authentication
{

   
    internal class RefactoredGoogleAuth : ObservableObject, IAuthenticationService
    {


        public string auth_uri { get; set; }
        public string scheme { get; set; }

        public string token_uri { get; set; }
        public string client_id { get; set; }
        public string auth_url { get; set; }
        public string callback_url { get; set; }


        public Account User { get; set; }

        public RefactoredGoogleAuth()
        {
            var scheme = "Google";


            client_id = "438312542461-555tgjs158r5jrj1vmvgfvrlccblg89a.apps.googleusercontent.com";
            auth_uri = "https://accounts.google.com/o/oauth2/auth";
            token_uri = "https://oauth2.googleapis.com/token";
            auth_url = $"{auth_uri}?response_type=code" +
                $"&redirect_uri=com.companyname.mauiapp8://" +
                $"&client_id={client_id}" +
                $"&scope=https://www.googleapis.com/auth/userinfo.profile" +
                $"&include_granted_scopes=true" +
                $"&state=state_parameter_passthrough_value";

            callback_url = "com.companyname.mauiapp8://";


            User = new Account();
        }

        public async Task<Account> AuthenticateAsync()
        {

            

            WebAuthenticatorResult result = null;


            var authUrl = new Uri($"{auth_url}{scheme}");
            var callbackUrl = new Uri(callback_url);

             result = await WebAuthenticator.AuthenticateAsync(authUrl, callbackUrl);



            var codeToken = result.Properties["code"];
            var parameters = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("grant_type","authorization_code"),
                new KeyValuePair<string,string>("client_id", client_id),
                new KeyValuePair<string,string>("redirect_uri", callback_url),
                new KeyValuePair<string,string>("code", codeToken),
            });

            using var client = new HttpClient();
            var accessTokenResponse = await client.PostAsync(token_uri, parameters);

            if (!accessTokenResponse.IsSuccessStatusCode)
            {
                throw new AuthenticationException("Failed to get access token");
            }

            var data = await accessTokenResponse.Content.ReadAsStringAsync();
            var loginResponse = JsonSerializer.Deserialize<LoginRespons>(data);
            var accessToken = loginResponse.id_token;

            try
            {
                return ValidateAccessToken(accessToken);
            }
            catch (Exception e)
            {
                throw new AuthenticationException("Failed to validate access token", e);
            }
        }

        public async Task SignOutAsync()
        {
            User = null;

        }

        private Account ValidateAccessToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(accessToken);
            var email = GetTokenClaim(token, "email");
            var name = GetTokenClaim(token, "name");
            var givenName = GetTokenClaim(token, "given_name");
            var picture = GetTokenClaim(token, "picture");
            var familyName = GetTokenClaim(token, "family_name");

            return new Account
            {
                Email = email,
                Name = name,
                GivenName = givenName,
                PictureUrl = picture,
                FamilyName = familyName,
                LoginSuccessful = true
            };
        }

        private string GetTokenClaim(JwtSecurityToken token, string claimType)
        {
            return token.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
        }

    }
}