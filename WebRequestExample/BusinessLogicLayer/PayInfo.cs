using System.Collections.Specialized;
using WebRequestExample.DataAccessLogicLayer;
using WebRequestExample.Model;

namespace WebRequestExample.BusinessLogicLayer
{
    public class PayInfo : IPayInfo
    {
        private ISendRequest DAL { get; set; }

        public string APIKey { get; set; }

        public PayInfo(string url, string apiKey)
        {
            APIKey = apiKey;
            DAL = new SendRequest(HttpMethodEnum.Post, url);
        }

        /// <summary>
        /// For unit test only
        /// </summary>
        /// <param name="dal"></param>
        public PayInfo(ISendRequest dal)
        {
            DAL = dal;
        }

        public string SendRequest<T>(T input) where T : class, new()
        {
            NameValueCollection parameters = new Signature(APIKey).ConvertToQueryString(input);
            string result = DAL.Create(parameters);

            return result;
        }
    }
}
