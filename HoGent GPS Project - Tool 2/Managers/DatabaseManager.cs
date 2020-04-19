using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HoGent_GPS_Project___Tool_2
{
    class DatabaseManager
    {
        public static List<Task> threads = new List<Task>();
        public static Dictionary<int, int> uploadProgressMax = new Dictionary<int, int>();
        public static Dictionary<int, int> uploadProgressStatus = new Dictionary<int, int>();

        public static void addState(int ID, String name)
        {
            MySqlConnection con = Program.db.getConnection();

            using var cmd = DatabaseUtil.CommandExecutor(con, "SELECT * FROM gps_states WHERE id = @id");
            cmd.Parameters.AddWithValue("@id", ID);

            con.Open();
            using MySqlDataReader rdr = cmd.ExecuteReader();
            if (!rdr.Read())
            {
                con.Close();
                using var cmd2 = DatabaseUtil.CommandExecutor(con, "INSERT INTO gps_states (id, name) VALUES (@id, @name)");
                cmd2.Parameters.AddWithValue("@id", ID);
                cmd2.Parameters.AddWithValue("@name", name);

                con.Open();
                cmd2.Prepare();
                cmd2.ExecuteNonQuery();
                con.Close();
            }
            con.Close();
        }

        public static void addCity(int ID, String name, int state)
        {
            MySqlConnection con = Program.db.getConnection();

            using var cmd = DatabaseUtil.CommandExecutor(con, "SELECT * FROM gps_cities WHERE id = @id");
            cmd.Parameters.AddWithValue("@id", ID);

            con.Open();
            using MySqlDataReader rdr = cmd.ExecuteReader();
            if (!rdr.Read())
            {
                con.Close();
                using var cmd2 = DatabaseUtil.CommandExecutor(con, "INSERT INTO gps_cities (id, name, state) VALUES (@id, @name, @state)");
                cmd2.Parameters.AddWithValue("@id", ID);
                cmd2.Parameters.AddWithValue("@name", name);
                cmd2.Parameters.AddWithValue("@state", state);

                con.Open();
                cmd2.Prepare();
                cmd2.ExecuteNonQuery();
                con.Close();
            }
            con.Close();
        }

        public static void addStreet(int ID, String name, double length, int city, String map)
        {
            MySqlConnection con = Program.db.getConnection();

            using var cmd = DatabaseUtil.CommandExecutor(con, "SELECT * FROM gps_streets WHERE id = @id");
            cmd.Parameters.AddWithValue("@id", ID);

            con.Open();
            using MySqlDataReader rdr = cmd.ExecuteReader();
            if (!rdr.Read())
            {
                con.Close();
                using var cmd2 = DatabaseUtil.CommandExecutor(con, "INSERT INTO gps_streets (id, name, length, city, map) VALUES (@id, @name, @length, @city, @map)");
                cmd2.Parameters.AddWithValue("@id", ID);
                cmd2.Parameters.AddWithValue("@name", name);
                cmd2.Parameters.AddWithValue("@length", length);
                cmd2.Parameters.AddWithValue("@city", city);
                cmd2.Parameters.AddWithValue("@map", map);

                con.Open();
                cmd2.Prepare();
                cmd2.ExecuteNonQuery();
                con.Close();
            }
            con.Close();
        }

        public static void uploadThread(String json)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ContractResolver = new DictionaryAsArrayResolver();

            DataManager.JsonProvincie state = JsonConvert.DeserializeObject<DataManager.JsonProvincie>(json, settings);
            uploadProgressMax.Add(state.ID, state.Cities.Count);
            uploadProgressStatus.Add(state.ID, 0);

            addState(state.ID, state.Name);
            foreach (DataManager.JsonGemeente city in state.Cities)
            {
                addCity(city.ID, city.Name, state.ID);

                foreach (DataManager.JsonStreet street in city.Streets)
                {
                    String map = "{}";
                    if (street.Graaf != null)
                    {
                        JsonSerializerSettings settings2 = new JsonSerializerSettings();
                        settings2.ContractResolver = new DictionaryAsArrayResolver();
                        map = JsonConvert.SerializeObject(street.Graaf, settings2);
                    }
                    addStreet(street.ID, street.Name, street.Length, city.ID, map);
                }

                uploadProgressStatus[state.ID] += 1;
            }
            state = null;
        }

        public static void uploadProgressThread()
        {
            while(true)
            {
                int count = 0;
                foreach (int key in uploadProgressMax.Keys)
                {
                    Program.drawTextProgressBar("Uploading data [State: " + key + "]", uploadProgressStatus[key], uploadProgressMax[key], 8 + (count * 2));
                    count += 1;
                }

                Thread.Sleep(20);
            }
        }

        public static void uploadData(DataManager.JsonData data)
        {
            Program.printHeader();
            Console.WriteLine("Loading data...");

            uploadProgressMax.Clear();
            uploadProgressStatus.Clear();

            foreach (String file in data.Files)
            {
                Console.WriteLine("- " + file);
                using (StreamReader r = new StreamReader(Program.appData + @"\data\" + file))
                {
                    string json = r.ReadToEnd();

                    threads.Add(Task.Run(() => uploadThread(json)));
                }
                
            }

            Thread.Sleep(50);
            Program.printHeader();
            Console.WriteLine("Uploading data...");
            var UploadProgressThread = Task.Run(() => uploadProgressThread());

            Task.WaitAll(threads.ToArray());
            UploadProgressThread.Wait();
        }

        class DictionaryAsArrayResolver : DefaultContractResolver
        {
            protected override JsonContract CreateContract(Type objectType)
            {
                if (objectType.GetInterfaces().Any(i => i == typeof(IDictionary) ||
                   (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>))))
                {
                    return base.CreateArrayContract(objectType);
                }

                return base.CreateContract(objectType);
            }
        }
    }
}
