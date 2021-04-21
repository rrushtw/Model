using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using WebRequestExample.Model;

namespace WebRequestExample.DataAccessLogicLayer
{
    public class SendRequest : ISendRequest
    {
        public string Url { get; set; }
        public HttpMethodEnum HttpMethod { get; set; }

        public SendRequest(HttpMethodEnum method, string url)
        {
            HttpMethod = method;
            Url = url;
        }

        public string Create(NameValueCollection parameters)
        {
            string result;

            HttpWebRequest request = WebRequest.Create(Url) as HttpWebRequest;
            request.Method = HttpMethod.ToString(); //方法
            request.KeepAlive = false; //是否保持連線
            request.ContentType = "application/x-www-form-urlencoded";

            byte[] bs = Encoding.UTF8.GetBytes(parameters.ToString());
            using (Stream reqStream = request.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
            }

            using (WebResponse response = request.GetResponse())
            {
                StreamReader sr = new StreamReader(response.GetResponseStream());
                result = sr.ReadToEnd();
                sr.Close();
            }

            return result;
        }
    }
}
