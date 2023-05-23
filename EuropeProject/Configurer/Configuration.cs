using System.Reflection;
using System.Text.Json;

namespace Configurer
{
    internal class Configuration : IConfiguration
    {
        private string FullFile;
        public Configuration(string path)
        {
            this.FullFile = File.ReadAllText(path);
        }
        public string GetConnectionString()
        {
            JsonDocument jsonDocument = JsonDocument.Parse(this.FullFile);
            JsonElement element = jsonDocument.RootElement;
            if (element.TryGetProperty("ConnectionString", out JsonElement x))
            {
                return x.ToString();
            }
            return null;
        }
    }
    public class Configurator
    {
        public static IConfiguration GetConfiguration()
        {
            string Path = "";
            string[] CurrentLocation = Directory.GetCurrentDirectory().Split('\\');
            string mainProject = Assembly.GetCallingAssembly().ToString().Split(',')[0];
            for(int i = 0; i < CurrentLocation.Length; i++)
            {
                Path += CurrentLocation[i] + '\\';
                if (CurrentLocation[i] == mainProject) break;
            }
            return new Configuration(Path + "AppConfiguration.json");
        }
    }
}
