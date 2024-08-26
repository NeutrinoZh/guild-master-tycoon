using UnityEngine;

namespace MTK.SaveSystem
{
    public class JSONSerializer : ISerializer
    {
        public string SerializeObject(object obj)
        {
            return JsonUtility.ToJson(obj);
        }

        public T DeserializeObject<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }
    }
}