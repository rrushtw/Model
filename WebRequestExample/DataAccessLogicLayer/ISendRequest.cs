using System.Collections.Specialized;

namespace WebRequestExample.DataAccessLogicLayer
{
    public interface ISendRequest
    {
        string Create(NameValueCollection parameters);
    }
}
