namespace WebRequestExample.BusinessLogicLayer
{
    public interface IPayInfo
    {
        string SendRequest<T>(T input) where T : class, new();
    }
}
