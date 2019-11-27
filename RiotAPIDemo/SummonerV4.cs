using System;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json;

namespace RiotAPIDemo
{
    class SummonerV4
    {

        public static readonly string ApiKey = "RGAPI-b68e4c84-c788-49b1-9d22-3fe64b301064";

        public static readonly Uri ApiBaseAddress = new Uri("https://na1.api.riotgames.com/lol/summoner/v4/summoners");
        public HttpClient Client { get; set; }

        public SummonerV4()
        {
            Client = new HttpClient
            {
                BaseAddress = ApiBaseAddress
            };
        }

        public Summoner GetSummonerByName(string summonerName)
        {
            var uriBuilder = new UriBuilder($"{ApiBaseAddress}/by-name/{summonerName}");
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["api_key"] = ApiKey;
            uriBuilder.Query = query.ToString();
            string requestUri = uriBuilder.ToString();

            var response = Client.GetAsync(requestUri).Result;
            if (response.IsSuccessStatusCode == false)
            {
                throw new Exception($"Request failed: {response.StatusCode}");
            }

            string jsonResponse = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Summoner>(jsonResponse);
        }
    }
}
