using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GOOM.TR.MyRetail.NET.Tests.Helpers
{
    public class TestClient
    {
        protected readonly HttpClient Client;
        protected readonly Uri BaseUri;

        public TestClient(HttpClient httpClient)
        {
            Client = httpClient;
            BaseUri = new Uri("http://localhost/");
        }

        public async Task<T> GetAsync<T>(string relativeUrl)
        {
            return await SendAsync<T>(relativeUrl, x => x.Method = HttpMethod.Get);
        }

        protected virtual HttpRequestMessage CreateHttpRequestMessage(string relativeUrl,
            Action<HttpRequestMessage> methodSetup, object content = null)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = BaseUri.Append(relativeUrl);
            request.Content = content != null ? CreateHttpContent(content) : null;

            return request;
        }


        private async  Task<T> SendAsync<T>(string relativeUrl,
            Action<HttpRequestMessage> methodSetup, object content = null)
        {
            var request = CreateHttpRequestMessage(relativeUrl, methodSetup, content);
            var response =  await Client.SendAsync(request);
            return await HandleResponseAsync<T>(request, response);
        }

        protected virtual async Task<T> HandleResponseAsync<T>(HttpRequestMessage request, HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var requestContent = await request.Content?.ReadAsStringAsync();
                throw new Exception($"Request failed: {request.RequestUri.AbsoluteUri} - {response.StatusCode} - {response.ReasonPhrase}:{Environment.NewLine}{requestContent}{Environment.NewLine}{responseContent}");
            }

            return responseContent.FromJson<T>();
        }

        private static HttpContent CreateHttpContent<T>(T content)
        {
            string json = content.ToJson();
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            return httpContent;
        }
    }
}