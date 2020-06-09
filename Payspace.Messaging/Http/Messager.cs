using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Payspace.Messaging.Http
{
    public class Messager : IMessager
    {
        public async Task<R> Get<R>(string url, string jwtToken = null)
        {
            return await ProcessUrlRequest<R>("GET", url, jwtToken);
        }
        public async Task<R> Put<T, R>(string url, T payload, string jwtToken = null)
        {
            return await Process<T,R>("PUT", url, payload, jwtToken);
        }

        public async Task<R> Post<T, R>(string url, T payload, string jwtToken = null)
        {
            return await Process<T, R>("POST", url, payload, jwtToken);
        }
        public async Task<R> Delete<R>(string url, string jwtToken=null)
        {
            return await ProcessUrlRequest<R>("DELETE", url, jwtToken);
        }
        public async Task<R> ProcessUrlRequest<R>(string verb, string url, string jwtToken = null)
        {
            R result = default(R);
            using (var handler = new HttpClientHandler())
            {
                //without this I have to set up a reverse proxy on my linux machine.
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (var client = new HttpClient(handler))
                {
                    SetHeaders(client, jwtToken);
                    HttpResponseMessage response;
                    verb = verb.ToUpper();
                    switch (verb)
                    {
                        case "GET":
                            response = await client.GetAsync(url);
                            break;
                        case "DELETE":
                            response = await client.DeleteAsync(url);
                            break;
                        default:
                            throw new Exception($"The verb {verb} is not supported using this method.");
                    }
                    


                    if (response.IsSuccessStatusCode)
                    {
                        var str = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<R>(str);
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                    return result;
                }
            }
        }
        private async Task<R> Process<T, R>(string verb, string url, T payload, string jwtToken = null)
        {
            R result = default(R);

            var json = JsonConvert.SerializeObject(payload);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            using (var handler = new HttpClientHandler())
            {
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (var client = new HttpClient(handler))
                {
                    SetHeaders(client, jwtToken);

                    HttpResponseMessage response;
                    if (verb == "POST")
                    {
                        response = await client.PostAsync(url, stringContent);
                    }
                    else if (verb == "PUT")
                    {
                        response = await client.PutAsync(url, stringContent);
                    }
                    else if (verb == "DELETE")
                    {
                        response = await client.DeleteAsync(url);
                    }
                    else
                    {
                        throw new Exception($"Unsupported verb '{verb}'.");
                    }


                    if (response.IsSuccessStatusCode && "POST|PUT|DELETE".Split('|').Contains(verb))
                    {
                        var str = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<R>(str);
                    }                    
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                    return result;
                }
            }
        }
        private void SetHeaders(HttpClient client, string jwtToken = null)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            if (!string.IsNullOrWhiteSpace(jwtToken))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{jwtToken}");
            client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
                    );
            client.DefaultRequestHeaders.Add("User-Agent", "Payspace.UI Client v1");
        }
    }
}

