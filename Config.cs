using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Plugin
{
    public class Config
    {
        // 是否启用
        public bool enable = true;

        // 防沉迷名单
        public List<string> names = new List<string>();

        public static Config Load(string path)
        {
            if (File.Exists(path))
            {
                return JsonConvert.DeserializeObject<Config>(File.ReadAllText(path));
            }
            else
            {
                var c = new Config();
                File.WriteAllText(path, JsonConvert.SerializeObject(c, Formatting.Indented));
                return c;
            }
        }

    }
}