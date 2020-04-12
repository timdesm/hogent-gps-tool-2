using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace HoGent_GPS_Project___Tool_2
{
    class DataManager
    {
        public class JsonData
        {
            public DateTimeOffset Date { get; set; }
            public String File { get; set; }
            public IList<int> States { get; set; }
            public IList<String> Files { get; set; }
        }

        public class JsonProvincie
        {
            public int ID { get; set; }
            public String Name { get; set; }
            public IList<JsonGemeente> Cities { get; set; }
        }

        public class JsonGemeente
        {
            public int ID { get; set; }
            public String Name { get; set; }
            public IList<JsonStreet> Streets { get; set; }
        }

        public class JsonStreet
        {
            public int ID { get; set; }
            public String Name { get; set; }
            public Double Length { get; set; }
            public JsonGraaf Graaf { get; set; }
        }

        public class JsonGraaf
        {
            public int ID { get; set; }
            public Dictionary<JsonKnoop, IList<JsonSegment>> Map { get; set; }
        }

        public class JsonKnoop
        {
            public int ID { get; set; }
            public JsonPunt Point { get; set; }
        }

        public class JsonSegment
        {
            public int ID { get; set; }
            public JsonKnoop Start { get; set; }
            public JsonKnoop End { get; set; }
            public IList<JsonPunt> Points { get; set; }
        }

        public class JsonPunt
        {
            public double X { get; set; }
            public double Y { get; set; }
        }

        public static JsonData importData(String file)
        {
            if(Directory.Exists(Program.appData + @"\data"))
                Directory.Delete(Program.appData + @"\data", true);
            Directory.CreateDirectory(Program.appData + @"\data\");

            ZipFile.ExtractToDirectory(file, Program.appData + @"\data");

            if(File.Exists(Program.appData + @"\data\data.json"))
            {
                using (StreamReader r = new StreamReader(Program.appData + @"\data\data.json"))
                {
                    string json = r.ReadToEnd();
                    JsonData data = JsonConvert.DeserializeObject<JsonData>(json);
                    return data;
                }
            }
            return null;
        }
    }
}
