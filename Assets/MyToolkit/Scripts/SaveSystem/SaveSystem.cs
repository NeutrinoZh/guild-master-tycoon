using System;
using System.IO;

namespace MTK.SaveSystem
{
    public class SaveSystem
    {
        private const string k_encryptionKey = "577d41f78860b6abee9e77fefaa8f22e74907d371f1dadc46a7c1a1cbcbf1004";

        private string _directory;
        private ISerializer _serializer;

        public SaveSystem(string directory, ISerializer serializer)
        {
            _directory = directory;
            _serializer = serializer;
        }

        public void Save<T>(string key, T value)
        {
            if (!typeof(T).IsSerializable)
                throw new InvalidOperationException($"{typeof(T).Name} must be serializable.");

            if (!Directory.Exists(_directory))
                Directory.CreateDirectory(_directory);

            var fullPath = Path.Combine(_directory, key);
            var serialized = _serializer.SerializeObject(value);
            // var encoded = Encoder.Encode(serialized, k_encryptionKey);

            using var stream = File.OpenWrite(fullPath);
            using var writer = new StreamWriter(stream);

            writer.Write(serialized);
        }

        public T Load<T>(string key)
        {
            if (!typeof(T).IsSerializable)
                throw new InvalidOperationException($"{typeof(T).Name} must be serializable.");

            if (!Directory.Exists(_directory))
                throw new InvalidOperationException($"{_directory} directory not exist");

            var fullPath = Path.Combine(_directory, key);

            var stream = File.OpenRead(fullPath);
            var reader = new StreamReader(stream);

            var loaded = reader.ReadToEnd();
            // var decoded = Encoder.Decode(loaded, k_encryptionKey);

            return _serializer.DeserializeObject<T>(loaded);
        }
    }
}