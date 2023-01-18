using Autofac;
using Credimujer.Op.Common.Exceptions;
using Credimujer.Op.Common.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Credimujer.Op.Service.Implementations.Base
{
    public class HttpClientService
    {
        private Lazy<IHttpClientFactory> factory;

        public HttpClientService(ILifetimeScope lifetimeScope)
        {
            this.factory = new Lazy<IHttpClientFactory>(() => lifetimeScope.Resolve<IHttpClientFactory>());
        }

        public async Task<T> InvokeAsync<T>(HttpMethod method, string endPoint, string user, string password, object parameters = null)
        {
            HttpClient client = factory.Value.CreateClient("ServiciosExterno");
            var byteArray = Encoding.ASCII.GetBytes(user + ":" + password);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            using (var request = new HttpRequestMessage(method, endPoint))
            {
                if (parameters != null && method != HttpMethod.Get)
                    request.Content = new System.Net.Http.StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json");

                using (var response = await client.SendAsync(request))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new TechnicalException(CommonResource.httpresponse_500);

                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                }
            }
        }

        public async Task<T> InvokeWithApiKeyAsync<T>(HttpMethod method, string endPoint, string nameApiKey, string apiKey, object parameters = null)
        {
            HttpClient client = factory.Value.CreateClient("ServiciosExterno");
            client.DefaultRequestHeaders.Add(nameApiKey, apiKey);
            using (var request = new HttpRequestMessage(method, endPoint))
            {
                if (parameters != null && method != HttpMethod.Get)
                    request.Content = new System.Net.Http.StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json");

                using (var response = await client.SendAsync(request))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new TechnicalException(CommonResource.httpresponse_500);

                    try
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                    
                }
            }
        }

        public async Task<List<T>> InvokeAsync2<T>(HttpMethod method, string endPoint, string user, string password, object parameters = null)
        {
            HttpClient client = factory.Value.CreateClient();
            var byteArray = Encoding.ASCII.GetBytes(user + ":" + password);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            using (var request = new HttpRequestMessage(method, endPoint))
            {
                if (parameters != null && method != HttpMethod.Get)
                    request.Content = new System.Net.Http.StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json");

                using (var response = await client.SendAsync(request))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new TechnicalException(CommonResource.httpresponse_500);

                    var content = await response.Content.ReadAsStringAsync();
                    //var content2 = JsonConvert.DeserializeObject<T>(content);

                    return JsonConvert.DeserializeObject<List<T>>(content);
                }
            }
        }

        public async Task<byte[]> InvokeAsyncToGetArchives(HttpMethod method, string endPoint, string user, string password, object parameters = null)
        {
            HttpClient client = factory.Value.CreateClient();
            var byteArray = Encoding.ASCII.GetBytes(user + ":" + password);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            using (var request = new HttpRequestMessage(method, endPoint))
            {
                if (parameters != null && method != HttpMethod.Get)
                    request.Content = new System.Net.Http.StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json");

                using (var response = await client.SendAsync(request))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new TechnicalException(CommonResource.httpresponse_500);

                    var content = await response.Content.ReadAsByteArrayAsync();
                    return content;
                }
            }
        }
    }
}