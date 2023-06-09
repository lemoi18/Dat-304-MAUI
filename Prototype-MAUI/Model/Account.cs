﻿using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiApp8.Model
{
    public class Account: ObservableObject
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("given_name")]
        public string GivenName { get; set; }

        [JsonPropertyName("family_name")]
        public string FamilyName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("picture")]
        public string PictureUrl { get; set; }

        public bool LoginSuccessful { get; set; }

        public int ExpiresIn { get; set; }

        public string RefreshToken { get; set; }
        public JwtSecurityToken Token { get; set; }

        public string AccessToken { get; set; }
        public string ErrorMessage { get; internal set; }
    }
}
