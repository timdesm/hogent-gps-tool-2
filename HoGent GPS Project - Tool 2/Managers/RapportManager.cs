using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HoGent_GPS_Project___Tool_2
{
    class RapportManager
    {
        public class JsonRapport
        {
            public DateTimeOffset Date { get; set; }
            public JsonCount Count { get; set; }
            public IList<JsonProvincie> States { get; set; }
        }

        public class JsonCount
        {
            public int States { get; set; }
            public int Cities { get; set; }
            public int Streets { get; set; }
        }

        public class JsonProvincie
        {
            public int ID { get; set; }
            public String Name { get; set; }
            public JsonProvincieCount Count { get; set; }
            public IList<JsonGemeente> Cities { get; set; }
        }

        public class JsonProvincieCount
        {
            public int Streets { get; set; }
            public int Cities { get; set; }
        }

        public class JsonGemeente
        {
            public int ID { get; set; }
            public String Name { get; set; }
            public int Streets { get; set; }
            public JsonStreet Longest { get; set; }
            public JsonStreet Shortest { get; set; }

        }

        public class JsonStreet
        {
            public int ID { get; set; }
            public String Name { get; set; }
            public Double Length { get; set; }
        }

        public static JsonRapport importRapport(String path)
        {
            using(StreamReader r = new StreamReader(path))
            {
                String data = r.ReadToEnd();
                JsonRapport rapport = JsonConvert.DeserializeObject<JsonRapport>(data);
                return rapport;
            }
        }
    }
}
