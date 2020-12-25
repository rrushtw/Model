using Newtonsoft.Json;
using System.Collections.Generic;

namespace Extensions
{
    public static class LinQExtension
    {
        public static IEnumerable<T> GetDistinct<T>(this IEnumerable<T> list)
        {
            List<T> resultList = new List<T>();
            List<string> stringList = new List<string>();

            foreach (T model in list)
            {
                string jsonString = JsonConvert.SerializeObject(model);

                if (stringList.Contains(jsonString)) continue;

                stringList.Add(jsonString);
                resultList.Add(model);
            }

            return resultList;
        }
    }
}
