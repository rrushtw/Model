using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebRequestExample.Model
{
    public class Signature
    {
        private string APIKey { get; set; }

        public Signature(string apiKey)
        {
            APIKey = apiKey;
        }

        public NameValueCollection ConvertToQueryString<T>(T input) where T : class, new()
        {
            NameValueCollection parameters = HttpUtility.ParseQueryString(string.Empty);

            foreach (var prop in input.GetType().GetProperties())
            {
                parameters.Add(prop.Name, prop.GetValue(input, null)?.ToString());
            }

            return parameters;
        }

        /// <summary>
        /// Sing data example with SHA384
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public NameValueCollection Sign<T>(T input) where T : class, new()
        {
            NameValueCollection postParams = HttpUtility.ParseQueryString(string.Empty);
            string stringToSign = string.Empty;

            #region implement sign rule here
            Dictionary<string, object> book = new Dictionary<string, object>();

            foreach (var prop in input.GetType().GetProperties())
            {
                book[prop.Name.ToLower()] = prop.GetValue(input, null)?.ToString();
            }

            book = book.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

            foreach (var pair in book)
            {
                if (pair.Value is null || Convert.ToString(pair.Value) == string.Empty)
                {
                    continue;
                }

                stringToSign += $"{pair.Key}={pair.Value}&";
                postParams.Add(pair.Key, pair.Value.ToString());
            }

            stringToSign = stringToSign.TrimEnd('&');
            string sign = HmacSha384(stringToSign);
            postParams.Add("sign", sign);
            #endregion

            return postParams;
        }

        private string HmacSha384(string stringToSign)
        {
            var encoding = new UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(APIKey);
            var messageBytes = encoding.GetBytes(stringToSign);
            var hmacSha384 = new HMACSHA384(keyByte);
            var hashMessage = hmacSha384.ComputeHash(messageBytes);
            return BitConverter.ToString(hashMessage).Replace("-", "").ToLower();
        }
    }
}
