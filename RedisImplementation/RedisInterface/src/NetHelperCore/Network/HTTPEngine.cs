using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetHelperCore.Network
{
    public class HTTPEngine<T> : IDisposable where T : INetworkMessage
    {
        private string _url;
        public HTTPEngine()
        { }
        public HTTPEngine(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new Exception("url cannot be empty");
            _url = url;
        }

        public async Task SendMessage(T request)
        {
            try
            {
                HttpWebRequest req = WebRequest.Create(_url) as HttpWebRequest;
                req.Proxy = null;
                req.KeepAlive = request.KeepAlive;
                req.Method = request.Verb.ToString();
                if (request.Headers != null && request.Headers.Count > 0)
                {
                    foreach (KeyValuePair<string, string> kvp in request.Headers)
                    {
                        req.Headers.Add(kvp.Key, kvp.Value);
                    }
                }
                byte[] buffer = Encoding.UTF8.GetBytes(request.RequestMessage());
                req.ContentLength = buffer.Length;
                req.ContentType = request.IsXml ? "text/xml" : "application/json";
                using (Stream PostData = req.GetRequestStream())
                {
                    await PostData.WriteAsync(buffer, 0, buffer.Length);
                    using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
                    {
                        if (resp != null)
                        {
                            request.StatusCode = (int)resp.StatusCode;
                            Encoding enc = Encoding.UTF8;
                            StreamReader loResponseStream =
                                new StreamReader(resp.GetResponseStream(), enc);
                            request.ResponseMessage = await loResponseStream.ReadToEndAsync();
                            loResponseStream.Close();
                            resp.Close();
                        }
                        else
                        {
                            request.Error = "Response is null";
                            request.StatusCode = (int)HttpStatusCode.NoContent;
                        }
                    }
                }
            }
            catch (TimeoutException te)
            {
                request.StatusCode = (int)HttpStatusCode.RequestTimeout;
                request.Error = te.Message;
            }
            catch (WebException we)
            {
                StreamReader loResponseStream =
                    new StreamReader(we.Response.GetResponseStream(), System.Text.Encoding.UTF8);
                string errMsgXml = loResponseStream.ReadToEnd();
                request.Error = errMsgXml;
                request.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            catch (Exception e)
            {
                request.Error = e.Message;
                request.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }
        public void SendSOAPMessage(T request, IDictionary<string, string> headers = null)
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.Proxy = null;
                    string method = request.IsXml ? "text/xml" : "text/json";
                    client.Headers.Add("Content-Type", $"{method}; charset=utf-8");
                    client.Headers.Add("SOAPAction", request.Verb.ToString());
                    if (headers != null && headers.Count > 0)
                    {
                        foreach (KeyValuePair<string, string> kvp in headers)
                        {
                            client.Headers.Add(kvp.Key, kvp.Value);
                        }
                    }
                    request.ResponseMessage = client.UploadString(_url, request.RequestMessage());

                }
            }
            catch (WebException we)
            {
                StreamReader loResponseStream =
                    new StreamReader(we.Response.GetResponseStream(), System.Text.Encoding.UTF8);
                string errMsgXml = loResponseStream.ReadToEnd();
                request.Error = errMsgXml;
            }
        }
        public void Dispose()
        {
        }
    }
}
