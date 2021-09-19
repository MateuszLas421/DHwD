using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DHwD;
using Newtonsoft.Json.Linq;

namespace MobilePanel.Helpers
{
    public static class AssemblyConfig
    {

        public static string FirebaseKey { get; private set; }
        public static string AppCenterKey { get; private set; }
        public static string FirebaseLink { get; private set; }

        public static async Task GetConfig()
        {
            var assembly = Assembly.GetAssembly(typeof(App));
            var resourceNames = assembly.GetManifestResourceNames();
            var configName = resourceNames.FirstOrDefault(s => s.Contains("Keys.json"));
            using (var resourceStream = assembly.GetManifestResourceStream(configName))
            {
                using (var reader = new StreamReader(resourceStream))
                {
                    var json = await reader.ReadToEndAsync();
                    var parse = JObject.Parse(json);
                    AppCenterKey = parse["AppCenterKey"].Value<string>();
                    FirebaseKey = parse["FirebaseKey"].Value<string>();
                    FirebaseLink = parse["FirebaseLink"].Value<string>();
                }
            }
        }
    }
}
