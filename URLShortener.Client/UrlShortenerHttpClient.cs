using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace URLShortener.Client
{
    public class UrlShortenerHttpClient
    {
        private HttpClient Client { get; set; }
        private Uri BaseAddress { get; set; }
        public UrlShortenerHttpClient(string baseAddress)
        {
            Client = new HttpClient();
            BaseAddress = new Uri(baseAddress);
        }

        public string GetTinyUrl(string longUrl)
        {
            var uri = new Uri(BaseAddress, "/api/Url/Encode");
            return PostAndGet<string, string>(uri, longUrl);
        }

        public string GetLongUrl(string shortUrl)
        {
            var query = String.Format("/api/Url/Decode/{0}", shortUrl);
            var uri = new Uri(BaseAddress, query);

            return QueryFor<string>(uri);
        }

        private G PostAndGet<P, G>(Uri queryUri, P content)
        {
            var json = JsonSerializer.Serialize<P>(content);

            var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            using (var post = Client.PostAsync(queryUri, httpContent))
            {
                return ProcessRequest<G>(post);
            }
        }

        private G QueryFor<G>(Uri queryUri)
        {
            using (var post = Client.GetAsync(queryUri))
            {
                return ProcessRequest<G>(post);
            }
        }


        private G ProcessRequest<G>(Task<HttpResponseMessage> post){
            if (post.Wait(10000))
                using (var response = post.Result)
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.OK:
                            var results = response.Content.ReadAsStringAsync().Result.ToString();
                            return JsonSerializer.Deserialize<G>(results);
                        case System.Net.HttpStatusCode.BadRequest:
                            throw new Exception("Error 400: Invalid input!");
                        case System.Net.HttpStatusCode.InternalServerError:
                            throw new Exception("Error 500: SERVER ERROR - Contact site administrator for help.");
                        default:
                            throw new Exception("UNKNOWN ERROR");
                    }
            else
                throw new Exception("Application timed out!");
        }
    }
}
