using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DBL
{
    public class RestApiClient : IDisposable
    {
        private string url;
        private RequestType requestType;
        private bool _disposed;
        private Dictionary<string, string> _headers = new Dictionary<string, string>();
        private HttpWebRequest webRequest;

        public RestApiClient(string url, RequestType requestType)
        {
            this.url = url;
            this.requestType = requestType;
        }

        public RestApiClient(string url, RequestType requestType, Dictionary<string, string> headers)
        {
            this.url = url;
            this.requestType = requestType;
            _headers = headers;
        }

        public async Task<HttpResult> SendRequestAsync(string requestData, Dictionary<string, string> keyValues = null, string mediaType = "application/json")
        {
            try
            {
                var handler = new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate };
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (var httpClient = new HttpClient(handler))
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    var contentType = new MediaTypeWithQualityHeaderValue(mediaType);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(contentType);
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "PesaPlug Api");

                    HttpContent httpContent = null;
                    if (requestType == RequestType.Post)
                    {
                        if (keyValues == null)
                            httpContent = new StringContent(requestData, Encoding.UTF8, "application/json");
                        else
                            httpContent = new FormUrlEncodedContent(keyValues);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(requestData))
                        {
                            var qParams = requestData.JsonToQuery();
                            url += qParams;
                        }
                    }

                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(url),
                        Method = requestType == RequestType.Post ? HttpMethod.Post : HttpMethod.Get,
                        Content = httpContent,
                    };

                    //---- Add headers
                    if (_headers != null)
                        foreach (var h in _headers)
                        {
                            request.Headers.Add(h.Key, h.Value);
                        }

                    //--- Send the request
                    var response = await httpClient.SendAsync(request);
                    var statusCode = (int)response.StatusCode;
                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        return new HttpResult { Success = true, Data = responseData, StatusCode = statusCode };
                    }
                    else
                    {
                        string reason = "";
                        string responseData = "";
                        if (statusCode == 400 || statusCode == 403 || statusCode == 409 || statusCode == 401)
                        {
                            responseData = await response.Content.ReadAsStringAsync();
                        }
                        else
                        {
                            responseData = await response.Content.ReadAsStringAsync();
                            reason = response.ReasonPhrase;
                        }

                        return new HttpResult
                        {
                            Success = false,
                            Exception = new Exception(reason),
                            StatusCode = statusCode,
                            Data = responseData
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new HttpResult { Success = false, Exception = ex };
            }
        }

        public string SendRequest(string requestData, out Exception error)
        {
            error = null;
            webRequest.Method = requestType == RequestType.Post ? "POST" : "GET";
            try
            {
                byte[] bytes = null;
                if (!string.IsNullOrEmpty(requestData))
                {
                    bytes = System.Text.Encoding.ASCII.GetBytes(requestData);
                    webRequest.ContentLength = bytes.Length;
                    using (Stream os = webRequest.GetRequestStream())
                    {
                        os.Write(bytes, 0, bytes.Length);
                    }
                }

                HttpWebResponse response;
                response = (HttpWebResponse)webRequest.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = response.GetResponseStream();
                    string responseStr = new StreamReader(responseStream).ReadToEnd();
                    return responseStr;
                }
                else
                {
                    error = new Exception(response.StatusDescription);
                    return "Error!";
                }
            }
            catch (Exception ex)
            {
                error = ex;
                webRequest.Abort();
                return string.Empty;
            }
            finally
            {

            }
        }

        public enum RequestType { Get = 0, Post = 1 }

        public class HttpResult
        {
            public int StatusCode { get; set; }
            public bool Success { get; set; }
            public string Data { get; set; }
            public Exception Exception { get; set; }
        }

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
            }
        }

        ~RestApiClient()
        {
            dispose(false);
        }
    }

    public static class ExtensionMethods
    {
        public static string JsonToQuery(this string jsonQuery)
        {
            string str = "?";
            str += jsonQuery.Replace(":", "=").Replace("{", "").
                        Replace("}", "").Replace(",", "&").
                            Replace("\"", "");
            return str;
        }
    }
}
