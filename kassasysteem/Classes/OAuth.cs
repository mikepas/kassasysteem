using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace kassasysteem.Classes
{
    class Token
    {
        public string access_token = null;
        public string expires_in = null;
        public string refresh_token = null;
    }

    class Me
    {
        public string CurrentDivision = null;
        public string FullName = null;
    }

    static class OAuth
    {
        public static String Code = null, State = null, AccessToken = null, RefreshToken = null, CurrentDivision = null, FullName = null;
        private static Double ExpiredAt = DateTime.UtcNow.Ticks / 10000000;

        //============ getAccess ===============================
        static public async Task getAccess()
        {
            if (Code == null)
            {
                throw new ExactError("Code NULL");
            }
            if (AccessToken == null)
            {
                await getToken();
            }
            if (CurrentDivision == null)
            {
                await getMe();
            }
            if ((DateTime.UtcNow.Ticks / 10000000) > ExpiredAt)
            {
                await getRefreshToken();
            }
        }

        //============ getToken ===============================

        static public async Task getToken()
        {
            Uri request = new Uri(Constants.BASE_URI + "/api/oauth2/token");

            HttpClient client = new HttpClient();

            String poststring = "grant_type=authorization_code" +
                                    "&code=" + Code +
                                    "&redirect_uri=" + Constants.CALLBACK_URL +
                                    "&client_id={" + Constants.CLIENT_ID + "}" +
                                    "&client_secret=" + Constants.CLIENT_SECRET;

            StringContent content = new StringContent(poststring, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
            HttpResponseMessage respons = await client.PostAsync(request, content);

            if (respons.IsSuccessStatusCode == false)
            {
                throw new ExactError("getToken Mislukt:  status = " + respons.StatusCode.ToString());
            }
            respons.EnsureSuccessStatusCode();
            string responsecontent = await respons.Content.ReadAsStringAsync();

            Token token = JsonConvert.DeserializeObject<Token>(responsecontent);

            AccessToken = token.access_token;
            RefreshToken = token.refresh_token;
            string ExpiresIn = token.expires_in;
            ExpiredAt = System.Convert.ToDouble(ExpiresIn) + DateTime.UtcNow.Ticks / 10000000;
        }

        //============ getRefreshToken ===============================

        static public async Task getRefreshToken()
        {
            Uri request = new Uri(Constants.BASE_URI + "/api/oauth2/token");

            HttpClient client = new HttpClient();

            String poststring = "grant_type=refresh_token" +
                                 "&refresh_token=" + OAuth.RefreshToken +
                                "&client_id={" + Constants.CLIENT_ID + "}" +
                                "&client_secret=" + Constants.CLIENT_SECRET;

            StringContent content = new StringContent(poststring, System.Text.Encoding.UTF8, "Content-Type: application/x-www-form-urlencoded");
            HttpResponseMessage respons = await client.PostAsync(request, content);

            if (respons.IsSuccessStatusCode == false)
            {
                throw new ExactError("refreshToken Mislukt:  status = " + respons.StatusCode.ToString());
            }
            respons.EnsureSuccessStatusCode();
            string responsecontent = await respons.Content.ReadAsStringAsync();

            Token token = JsonConvert.DeserializeObject<Token>(responsecontent);

            AccessToken = token.access_token;
            RefreshToken = token.refresh_token;
            string ExpiresIn = token.expires_in;
            ExpiredAt = System.Convert.ToDouble(ExpiresIn) + DateTime.UtcNow.Ticks / 10000000;
        }

        //============ getMe ===============================

        static public async Task getMe()
        {
            if ((DateTime.UtcNow.Ticks / 10000000) > ExpiredAt) await getRefreshToken();

            Uri request = new Uri(Constants.BASE_URI + "/api/v1/current/Me?access_token=" + OAuth.AccessToken);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

            HttpResponseMessage respons = await client.GetAsync(request);
            if (respons.IsSuccessStatusCode == false)
            {
                throw new ExactError("getMe Mislukt:  status = " + respons.StatusCode.ToString());
            }
            respons.EnsureSuccessStatusCode();
            string responsecontent = await respons.Content.ReadAsStringAsync();

            JObject content = JObject.Parse(responsecontent);
            IList<JToken> results = content["d"]["results"].Children().ToList();
            JToken result = results[0];
            Me me = JsonConvert.DeserializeObject<Me>(results[0].ToString());
            CurrentDivision = me.CurrentDivision;
            FullName = me.FullName;
        }
    }
}
