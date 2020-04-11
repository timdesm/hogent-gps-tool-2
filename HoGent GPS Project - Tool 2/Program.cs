using System;
using System.IO;

namespace HoGent_GPS_Project___Tool_2
{
    class Program
    {
        public static RapportManager.JsonRapport rapport;
        public static DataManager.JsonData data;

        public static String appData;

        static void Main(string[] args)
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            appData = Path.Combine(appDataPath, @"TimDeSmet-HoGent\GPS-Project\Tool-2");

            printHeader();
            Console.Write("Rapport file path?: ");
            String rapportFile = Console.ReadLine();

            printHeader();
            Console.WriteLine("Loading rapport...");
            Console.WriteLine(" ");

            rapport = RapportManager.importRapport(rapportFile);


            while(data == null)
            {
                printHeader();
                Console.Write("Data file path?: ");
                String dataFile = Console.ReadLine();

                printHeader();
                Console.WriteLine("Loading data...");
                Console.WriteLine(" ");

                data = DataManager.importData(dataFile);

                if(data == null)
                {
                    printHeader();
                    Console.WriteLine("File was not a valid Data file");
                    Console.WriteLine(" ");
                    Console.Write("Press ENTER to continue...");
                    Console.ReadLine();
                }
            }

            printHeader();
            Console.WriteLine("All data has been loaded");
            Console.WriteLine(" ");

            while (true)
            {
                Console.WriteLine("Rapport streets: " + rapport.Count.Streets);
                Console.WriteLine("Data states: " + data.States.Count);
                Console.ReadLine();
            }
        }

        public static void printHeader()
        {
            Console.Clear();
            Console.WriteLine("--------------------------");
            Console.WriteLine("Project created by Tim De Smet");
            Console.WriteLine("HoGent GPS - Tool 2");
            Console.WriteLine("--------------------------");
            Console.WriteLine(" ");
        }
    }
}
