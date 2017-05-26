using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace WemManagementStudio
{
    public static class Serializer
    {
        private static string SettingsFile = "Settings.xml";

        public static void Save(Settings settings)
        {
            using (var writer = new XmlTextWriter(File.Open(SettingsFile, FileMode.Create), Encoding.Unicode))
            {
                var serializer = new XmlSerializer(typeof(Settings));
                serializer.Serialize(writer, settings);
            }
        }

        public static Settings Load()
        {
            if (!File.Exists(SettingsFile))
            {
                return new Settings();
            }

            using (var stream = File.Open(SettingsFile, FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(Settings));
                return (Settings)serializer.Deserialize(stream);
            }
        }
    }
}
