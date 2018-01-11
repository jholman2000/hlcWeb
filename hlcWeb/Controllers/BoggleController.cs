using System;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SimpleJson;

namespace hlcWeb.Controllers
{
    public class BoggleController : Controller
    {
        [HttpPost]
        [AllowAnonymous]
        public string LookupWord(string wordId)
        {
            string lookupResults = null;
            var client = new RestClient();
            IRestResponse response;

            client.BaseUrl = new Uri($"https://od-api.oxforddictionaries.com:443/api/v1/entries/en/{wordId}/definitions");

            var request = new RestRequest {Method = Method.GET};
            request.AddHeader("Accept", "application/json");
            request.AddHeader("app_id", "41d2606f");
            request.AddHeader("app_key", "ac3ee153ab384196c4bc7045b180d7e5");

            response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                lookupResults = response.Content;
            }
            else
            {
                // Searching for the word did not find an exact match.  Check to see if this is a form of
                // a root word by checking the Oxford Lemmatron
                client.BaseUrl = new Uri($"https://od-api.oxforddictionaries.com:443/api/v1/inflections/en/{wordId}");
                response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    // Not a valid word or form of a word
                    lookupResults = "NOTFOUND";
                }
                else
                {
                    // Found the root word.  
                    var responseJson = JObject.Parse(response.Content);
                    string rootWord = responseJson["results"][0]["lexicalEntries"][0]["inflectionOf"][0]["text"].ToString();

                    lookupResults = $"<{rootWord}>";

                    client.BaseUrl = new Uri($"https://od-api.oxforddictionaries.com:443/api/v1/entries/en/{rootWord}/definitions");
                    response = client.Execute(request);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        lookupResults += response.Content;
                    }
                }

            }
            return lookupResults;
        }

    }
}