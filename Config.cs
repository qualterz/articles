using Newtonsoft.Json;
using System;
using System.IO;

namespace Articles
{
    [Serializable]
    public class Config
    {
        public static Config Instance { get; set; }
            = new Config();

        public string Username { get; set; }

        public string DatabasePath { get; set; }

        public string Theme { get; set; }
        public string Color { get; set; }

        public void Load(string path)
        {
            if (File.Exists(path))
            {
                string raw = File.ReadAllText(path);

                Instance = JsonConvert.DeserializeObject<Config>(raw);
            }
        }

        public void Save(string path)
        {
            var raw = JsonConvert.SerializeObject(this, Formatting.Indented);

            File.Create(path).Dispose();
            File.WriteAllText(path, raw);
        }

        private Config() { }
    }
}
