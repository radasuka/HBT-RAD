using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HBT_RAD
{
    public static class OAuth
    {
        private static Keys _tokens = null;
        public static Keys Tokens => _tokens ?? (_tokens = ReadTokens());

        public class Keys
        {
            public string ConsumerKey { get; set; }
            public string ConsumerSecret { get; set; }
            public string AccessToken { get; set; }
            public string AccessSecret { get; set; }
        }

        private static Keys ReadTokens()
        {
            var json = File.ReadAllText("../../AppConfig/keys.json");
            return JsonConvert.DeserializeObject<Keys>(json);
        }
    }
}
