using Azure;
using Newtonsoft.Json;
using OnlineStore.Models;
using OnlineStoreMvc.Models;
using OnlineStoreMvc.Services.IServices;
using System.Text;

namespace OnlineStoreMvc.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get; set; }
        IHttpClientFactory httpClient { get; set; }

        public BaseService(IHttpClientFactory HttpClient)
        {
            responseModel = new();
            httpClient = HttpClient;
        }

        public async Task<T> SendAsync<T>(APIRequest request)
        {
            try
            {
                var client = httpClient.CreateClient();
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.Headers.Add("Accept", "application/json");
                requestMessage.RequestUri = new Uri(request.Url);

                //if sending data to API
                if (request.Data != null)
                {
                    requestMessage.Content = new StringContent(JsonConvert.SerializeObject(request.Data),
                        Encoding.UTF8, "application/json");
                }

                switch (request.ApiType)
                {
                    case OnlineStore.Utility.SD.ApiType.POST:
                        requestMessage.Method = HttpMethod.Post;
                        break;
                    case OnlineStore.Utility.SD.ApiType.PUT:
                        requestMessage.Method = HttpMethod.Put;
                        break;
                    case OnlineStore.Utility.SD.ApiType.DELETE:
                        requestMessage.Method = HttpMethod.Delete;
                        break;
                    default:
                        requestMessage.Method = HttpMethod.Get;
                        break;
                }

                HttpResponseMessage response = null;
                response = await client.SendAsync(requestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();
                var APIResponse = JsonConvert.DeserializeObject<T>(responseContent);

                return APIResponse;
            }
            catch (Exception ex)
            {
                APIResponse errorResponse = new APIResponse
                {
                    isSuccess = false,
                    ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("error", ex.ToString()) }
            };
                
                //have to make it generic in order to return
                return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(errorResponse));

                //var res = JsonConvert.SerializeObject(errorResponse);
                //var APIResponse = JsonConvert.DeserializeObject<T>(res);
            }
        }
    }
}
