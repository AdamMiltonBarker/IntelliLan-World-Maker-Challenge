using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using Windows.Web.Http.Headers;

namespace TIA
{
    class TechBubbleWeb
    {

        public async Task<JObject> jsonApiCall(string[] arr)
        {

            System.Diagnostics.Debug.WriteLine("Starting request");
            HttpBaseProtocolFilter filter = new HttpBaseProtocolFilter();
            filter.CacheControl.WriteBehavior = HttpCacheWriteBehavior.NoCache;
            filter.CacheControl.ReadBehavior = HttpCacheReadBehavior.MostRecent;
            filter.AllowUI = false;
            HttpClient httpClient = new HttpClient(filter);
            var headers = httpClient.DefaultRequestHeaders;
            if (arr[4] != "")
            {
                System.Diagnostics.Debug.WriteLine("Adding Auth Header");
                System.Diagnostics.Debug.WriteLine(arr[4]);
                headers.Authorization = new HttpCredentialsHeaderValue("SharedAccessSignature", arr[4]);
            }

            Uri requestUri = new Uri(arr[0] + arr[1]);
            System.Diagnostics.Debug.WriteLine(requestUri);
            HttpResponseMessage httpResponse = new HttpResponseMessage();

            string httpResponseBody = "";

            httpResponse = await httpClient.GetAsync(requestUri);
            httpResponse.EnsureSuccessStatusCode();
            httpResponseBody = await httpResponse.Content.ReadAsStringAsync();

            System.Diagnostics.Debug.WriteLine(httpResponseBody);
            var jsreader = new JsonTextReader(new StringReader(httpResponseBody));
            var jsonResponse = (JObject)new JsonSerializer().Deserialize(jsreader);
            return jsonResponse;
        }

        public async Task<JObject> jsonApiPost(string[] arr)
        {

            HttpClient httpClient = new HttpClient();
            var headers = httpClient.DefaultRequestHeaders;

            Uri requestUri = new Uri(arr[0] + arr[1]);
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            IHttpContent content = new HttpStringContent(arr[2]);
            content.Headers.ContentType = new HttpMediaTypeHeaderValue("application/x-www-form-urlencoded");
            System.Diagnostics.Debug.WriteLine(content);
            string httpResponseBody = "";
            httpResponse = await httpClient.PostAsync(requestUri, content);
            httpResponse.EnsureSuccessStatusCode();
            httpResponseBody = await httpResponse.Content.ReadAsStringAsync();

            var jsreader = new JsonTextReader(new StringReader(httpResponseBody));
            var jsonResponse = (JObject)new JsonSerializer().Deserialize(jsreader);
            return jsonResponse;
        }
    }
}
