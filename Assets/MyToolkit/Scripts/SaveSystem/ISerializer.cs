namespace MTK.SaveSystem
{
    public interface ISerializer
    {
        public string SerializeObject(object obj);
        public T DeserializeObject<T>(string serialized);
    }
}