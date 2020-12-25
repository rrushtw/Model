using Newtonsoft.Json;

namespace Extensions
{
    public static class ObjectClassExtension
    {
        public static bool IsEqual<T>(this T obj1, T obj2) where T : class, new()
        {
            //If both class are null, the result is equal
            if (obj1 is null && obj2 is null)
            {
                return true;
            }

            /* It counld not be both null is this case.
             * If any null exists, the result is false.
             * That means both class must have container.
             */
            if (obj1 is null || obj2 is null)
            {
                return false;
            }

            if (obj1.GetType() != obj2.GetType())
            {
                return false;
            }

            string json1 = JsonConvert.SerializeObject(obj1);
            string json2 = JsonConvert.SerializeObject(obj2);

            bool isEqual = json1 == json2;
            return isEqual;
        }
    }
}
