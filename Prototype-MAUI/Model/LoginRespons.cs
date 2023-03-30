using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace MauiApp8.Model
{
    public class LoginRespons
    {
        public string access_token { get; set; }
        public string id_token { get; set; }

        public int expires_in { get; set; }

        public string refresh_token { get; set; }

        public string scope { get; set; }

        public string token_type { get; set; }
        public string email { get; set; }


    }
}
