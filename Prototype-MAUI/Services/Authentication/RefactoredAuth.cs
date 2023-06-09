﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp8.Model;
using System.Text.Json;
using System.Linq.Expressions;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Json;

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

        public JwtSecurityToken Token { get; set; }

        public string AccessToken { get; set; }

        public Account User { get; set; }

        public RefactoredGoogleAuth()
        {
             scheme = "Google";


            client_id = "438312542461-555tgjs158r5jrj1vmvgfvrlccblg89a.apps.googleusercontent.com";
            auth_uri = "https://accounts.google.com/o/oauth2/auth";
            token_uri = "https://oauth2.googleapis.com/token";
            auth_url = $"{auth_uri}?response_type=code" +
                $"&redirect_uri=com.companyname.mauiapp8://" +
                $"&client_id={client_id}" +
                $"&scope=https://www.googleapis.com/auth/userinfo.profile%20https://www.googleapis.com/auth/fitness.activity.read%20https://www.googleapis.com/auth/fitness.activity.write" +
                $"&include_granted_scopes=true" +
                 $"&access_type=offline" +
                $"&state=state_parameter_passthrough_value";
            
            callback_url = "com.companyname.mauiapp8://";
            Token = null;

            User = new Account();
        }
        public async Task<Account> RefreshAuthenticateAsync()
        {
          

            // If we have a refresh token, use it to obtain a new access token.
            if (!string.IsNullOrEmpty(User.RefreshToken))
            {
                var parameters = new FormUrlEncodedContent(new[]
                {
            new KeyValuePair<string,string>("grant_type","refresh_token"),
            new KeyValuePair<string,string>("client_id", client_id),
            new KeyValuePair<string,string>("refresh_token", User.RefreshToken),
            });

                using var client = new HttpClient();
                var accessTokenResponse = await client.PostAsync(token_uri, parameters);

                if (!accessTokenResponse.IsSuccessStatusCode)
                {
                    throw new AuthenticationException("Failed to get access token");
                }

                var data = await accessTokenResponse.Content.ReadFromJsonAsync<TokenResponse>();
                //var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(data);

                Console.WriteLine(data.access_token);

                // Validate and return the user account.
                return ValidateAccessToken(data.id_token, data.access_token, data.refresh_token, data.expires_in);
            }

            return User;
        }

        public async Task<Account> AuthenticateAsync(CancellationToken cancellationToken)
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
            var accessTokenResponse = await client.PostAsync(token_uri, parameters, cancellationToken);

            if (!accessTokenResponse.IsSuccessStatusCode)
            {
                throw new AuthenticationException("Failed to get access token");
            }

            var data = await accessTokenResponse.Content.ReadAsStringAsync();
            var loginResponse = JsonSerializer.Deserialize<LoginRespons>(data);
            Console.WriteLine(loginResponse.access_token);

            try
            {


                return ValidateAccessToken(loginResponse.id_token, loginResponse.access_token, loginResponse.refresh_token, loginResponse.expires_in);



            }
            catch (Exception e)
            {
                throw new AuthenticationException("Failed to validate access token", e);
            }
        }

        public async Task SignOutAsync()
        {
            User = null;
            Token = null;

            await Task.CompletedTask;
        }

        private Account ValidateAccessToken(string id_token, string access_token, string refresh_token, int expires_in)
        {
            var handler = new JwtSecurityTokenHandler();
            try
            {
                var token = handler.ReadJwtToken(id_token);
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
                    LoginSuccessful = true,
                    Token = token,
                    AccessToken = access_token,
                    RefreshToken = refresh_token,
                    ExpiresIn = expires_in,
                };
            }
            catch (SecurityTokenException ex)
            {
                // Log the error message

                // Return an error response with a specific message based on the exception type
                if (ex is SecurityTokenExpiredException)
                {
                    return new Account
                    {
                        LoginSuccessful = false,
                        ErrorMessage = "The access token has expired. Please log in again."
                    };
                }
                else if (ex is SecurityTokenSignatureKeyNotFoundException)
                {
                    return new Account
                    {
                        LoginSuccessful = false,
                        ErrorMessage = "The access token signature could not be verified. Please log in again."
                    };
                }
                else
                {
                    return new Account
                    {
                        LoginSuccessful = false,
                        ErrorMessage = "An error occurred while validating the access token. Please try again later."
                    };
                }
            }


        }



        private string GetTokenClaim(JwtSecurityToken token, string claimType)
        {
            return token.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
        }



    }
}