using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            string filter = "&$filter=";
            if (itemGroupDescription != null)
            {
                filter += "substringof('" + Uri.EscapeDataString(itemGroupDescription) + "',ItemGroupDescription)+eq+true";
            }
            if (description != null)
            {
                filter += "+and+substringof('" + Uri.EscapeDataString(description) + "',Description)+eq+true";
            }
            if (!string.IsNullOrEmpty(code))
            {
                filter += "+and+Code eq '" + code + "'";
            }
            if (barcode != null)
            {
                //filter += "+and+substringof('" + Uri.EscapeDataString(barcode) + "',Barcode)+eq+true";
            }
            string orderby = "&$orderby=Description+asc";
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
                Items searchResult = JsonConvert.DeserializeObject<Items>(result.ToString());
                searchResults.Add(searchResult);
            }
            return searchResults;
        }

        public static async Task<String> getItemPrice(string id = "")
        {
            await OAuth.getAccess();
            //string filter = "&$filter=substringof('" + Uri.EscapeDataString(id) + "',ID)+eq+true";
            string select = "&$select=SalesPrice";
            Uri request = new Uri(Constants.BASE_URI + "/api/v1/" + OAuth.CurrentDivision + "/read/logistics/ItemDetailsByID?access_token=" + OAuth.AccessToken + "&itemId=guid'" + id + "'" + select);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

            HttpResponseMessage respons = await client.GetAsync(request);
            if (respons.IsSuccessStatusCode == false)
            {
                throw new ExactError("getItemPrice Mislukt:  status = " + respons.StatusCode.ToString());
            }
            respons.EnsureSuccessStatusCode();
            string responsecontent = await respons.Content.ReadAsStringAsync();

            JObject content = JObject.Parse(responsecontent);
            IList<JToken> results = content["d"]["results"].Children().ToList();

            var searchResults = "";
            foreach (JToken result in results)
            {
                Items searchResult = JsonConvert.DeserializeObject<Items>(result.ToString());
                searchResults = searchResult.SalesPrice;
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
                ItemGroups searchResult = JsonConvert.DeserializeObject<ItemGroups>(result.ToString());
                searchResults.Add(searchResult);
            }
            return searchResults;
        }

        public static async Task<List<Customer>> getCustomers(string f = "")
        {
            await OAuth.getAccess();

            string filter = "&$filter=Type eq 'A'+and+Remarks eq '" + f + "'";
            string select = "&$select=Name,Phone";
            Uri request = new Uri(Constants.BASE_URI + "/api/v1/" + OAuth.CurrentDivision + "/crm/Accounts?access_token=" + OAuth.AccessToken + filter + select);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

            HttpResponseMessage respons = await client.GetAsync(request);
            if (respons.IsSuccessStatusCode == false)
            {
                throw new ExactError("getCustomers Mislukt:  status = " + respons.StatusCode.ToString());
            }
            respons.EnsureSuccessStatusCode();
            string responsecontent = await respons.Content.ReadAsStringAsync();

            JObject content = JObject.Parse(responsecontent);
            IList<JToken> results = content["d"]["results"].Children().ToList();

            List<Customer> searchResults = new List<Customer>();
            foreach (JToken result in results)
            {
                searchResults.Add(JsonConvert.DeserializeObject<Customer>(result.ToString()));
            }
            return searchResults;
        }
    }
}
