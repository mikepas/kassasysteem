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
        public static async Task<List<Items>> getItems(string itemGroupDescription = "", string description = "", string code = null, string barcode = null)
        {
            await OAuth.getAccess();

            string filter = "&$filter=substringof('" + itemGroupDescription + "',ItemGroupDescription)+eq+true+and+substringof('" + description + "',Description)+eq+true+and+substringof('" + code + "',Code)+eq+true";
            if (barcode != null)
            {
                filter += "+and+substringof('" + barcode + "',Barcode)+eq+true";
            }
            string orderby = "&$orderby=Code+asc";
            Uri request = new Uri(Constants.BASE_URI + "/api/v1/" + OAuth.CurrentDivision + "/logistics/Items?access_token=" + OAuth.AccessToken + filter + orderby);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

            HttpResponseMessage respons = await client.GetAsync(request);
            if (respons.IsSuccessStatusCode == false)
            {
                throw new ExactError("getItems Mislukt:  status = " + respons.StatusCode.ToString());
            }
            respons.EnsureSuccessStatusCode();
            string responsecontent = await respons.Content.ReadAsStringAsync();

            JObject content = JObject.Parse(responsecontent);
            IList<JToken> results = content["d"]["results"].Children().ToList();

            List<Items> searchResults = new List<Items>();
            foreach (JToken result in results)
            {
                var x = result;
                Items searchResult = JsonConvert.DeserializeObject<Items>(result.ToString());
                searchResults.Add(searchResult);
            }
            return searchResults;
        }

        public static async Task<List<ItemGroups>> getItemGroups(string f = "")
        {
            await OAuth.getAccess();

            string filter = "&$filter=substringof('" + f + "',Description)+eq+true";
            string orderby = "&$orderby=Code+asc";
            Uri request = new Uri(Constants.BASE_URI + "/api/v1/" + OAuth.CurrentDivision + "/logistics/ItemGroups?access_token=" + OAuth.AccessToken + filter + orderby);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

            HttpResponseMessage respons = await client.GetAsync(request);
            if (respons.IsSuccessStatusCode == false)
            {
                throw new ExactError("getItemGroup Mislukt:  status = " + respons.StatusCode.ToString());
            }
            respons.EnsureSuccessStatusCode();
            string responsecontent = await respons.Content.ReadAsStringAsync();

            JObject content = JObject.Parse(responsecontent);
            IList<JToken> results = content["d"]["results"].Children().ToList();

            List<ItemGroups> searchResults = new List<ItemGroups>();
            foreach (JToken result in results)
            {
                var x = result;
                ItemGroups searchResult = JsonConvert.DeserializeObject<ItemGroups>(result.ToString());
                searchResults.Add(searchResult);
            }
            return searchResults;
        }

    }
}
