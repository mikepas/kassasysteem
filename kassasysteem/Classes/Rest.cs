using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace kassasysteem.Classes
{
    class Rest
    {
        //============ getGLAccounts ===============================

        /*
        static public async Task<List<GLAccount>> getGLAccounts(string f = "")   // In dit voorbeeld kan het ook met List ipv ObservableCollection
        {
            await OAuth.getAccess();

            string filter = "&$filter=substringof('" + f + "',Description)+eq+true";
            string orderby = "&$orderby=Code+asc";
            Uri request = new Uri(Constants.BASE_URI + "/api/v1/" + OAuth.CurrentDivision + "/financial/GLAccounts?access_token=" + OAuth.AccessToken + filter + orderby);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

            HttpResponseMessage respons = await client.GetAsync(request);
            if (respons.IsSuccessStatusCode == false)
            {
                throw new ExactError("getGLAccounts Mislukt:  status = " + respons.StatusCode.ToString());
            }
            respons.EnsureSuccessStatusCode();
            string responsecontent = await respons.Content.ReadAsStringAsync();

            JObject content = JObject.Parse(responsecontent);
            IList<JToken> results = content["d"]["results"].Children().ToList();

            List<GLAccount> searchResults = new List<GLAccount>();
            foreach (JToken result in results)
            {
                var x = result;
                GLAccount searchResult = JsonConvert.DeserializeObject<GLAccount>(result.ToString());
                searchResults.Add(searchResult);
            }
            return searchResults;
        }
        */
    }
}
