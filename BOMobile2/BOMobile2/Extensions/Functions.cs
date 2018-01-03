using Newtonsoft.Json;
using System.IO;
using System.Reflection;

namespace Extensions
{
    public class Functions
    {
        public static T ReadFromFile<T>()
        {
            var assembly = typeof(Functions).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("BOMobile2.Language.txt");
            string text = "";
            using (var reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<T>(text);
        }

        public static void WriteToFile<T>()
        {

        }
    }
}
