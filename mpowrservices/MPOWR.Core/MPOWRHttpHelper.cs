using System.Net.Http;
using System.Net.Http.Headers;

namespace MPOWR.Core
{
    public class MPOWRHttpHelper
    {
        public static HttpResponseMessage Get(string url)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
                HttpResponseMessage response =  client.GetAsync(url).Result;
                return response;
            }
        }

        public static HttpResponseMessage Get(string url,string userHeader)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("UserId", userHeader);
                HttpResponseMessage response = client.GetAsync(url).Result;
                return response;
            }
        }

        public static HttpResponseMessage Post(string url,HttpContent httpContent)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.PostAsync(url, httpContent).Result; 
                return response;
            }
        }

        public static HttpResponseMessage Put(string url, HttpContent httpContent)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.PutAsync(url, httpContent).Result;
                return response;
            }
        }

        public static HttpResponseMessage Delete(string url)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.DeleteAsync(url).Result;
                return response;
            }
        }
    }
}
