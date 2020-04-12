using System;
using System.IO;
using System.Threading;

namespace HoGent_GPS_Project___Tool_2
{
    class Program
    {
        public static RapportManager.JsonRapport rapport;
        public static DataManager.JsonData data;

        public static String appData;

        public static DatabaseUtil db;
        public static String mysql_host = "timdesmet.be";
        public static String mysql_user = "u32002p26917_hogent";
        public static String mysql_pass;
        public static String mysql_data = "u32002p26917_hogent";

        static void Main(string[] args)
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            appData = Path.Combine(appDataPath, @"TimDeSmet-HoGent\GPS-Project\Tool-2");

            Boolean isConnected = false;
            while (!isConnected)
            {
                printHeader();
                Console.Write("Database password?: ");
                mysql_pass = Console.ReadLine();
                db = new DatabaseUtil(mysql_host, mysql_user, mysql_pass, mysql_data);

                int status = db.checkConnection();
                switch (status)
                {
                    case 1:
                        isConnected = true;
                        break;
                    case 1042:
                        Console.WriteLine("Unabale to create connection!");
                        Console.WriteLine(" ");
                        Console.Write("Press ENTER to continue...");
                        Console.ReadLine();
                        break;
                    case 0:
                        Console.WriteLine("Invalid password!");
                        Console.WriteLine(" ");
                        Console.Write("Press ENTER to continue...");
                        Console.ReadLine();
                        break;
                    default:
                        break;
                }
            }

            while (data == null)
            {
                printHeader();
                Console.Write("Data file path?: ");
                String dataFile = Console.ReadLine();

                printHeader();
                Console.WriteLine("Loading data...");
                Console.WriteLine(" ");

                data = DataManager.importData(dataFile);

                if (data == null)
                {
                    printHeader();
                    Console.WriteLine("File was not a valid Data file");
                    Console.WriteLine(" ");
                    Console.Write("Press ENTER to continue...");
                    Console.ReadLine();
                }
            }

            Thread.Sleep(25);
            while (true)
            {
                printHeader();
                Console.WriteLine("All data has been loaded");
                Console.WriteLine(" ");

                Console.WriteLine("----- [MENU] -----");
                Console.WriteLine("[1] UPLOAD TO DATABASE");
                Console.WriteLine("[2] DATABASE STATUS");
                Console.Write("Selection: ");
                String selection = Console.ReadLine();

                switch (selection)
                {
                    case "1":
                        MenuManager.case1();
                        break;
                    case "2":
                        MenuManager.case2();
                        break;
                    default:
                        Console.Write("Wrong selection input, press ENTER to continue...");
                        Console.ReadLine();
                        break;
                }
            }
        }

        public static void drawTextProgressBar(string stepDescription, int progress, int total, int curstorTop)
        {
            int totalChunks = 50;

            Console.CursorLeft = 0;
            Console.CursorTop = curstorTop;
            Console.Write("[");
            Console.CursorLeft = totalChunks + 1;
            Console.Write("]");
            Console.CursorLeft = 1;

            double pctComplete = Convert.ToDouble((int)progress) / total;
            int numChunksComplete = Convert.ToInt16(totalChunks * pctComplete);

            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write("".PadRight(numChunksComplete));

            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write("".PadRight(totalChunks - numChunksComplete));

            Console.CursorLeft = totalChunks + 5;
            Console.BackgroundColor = ConsoleColor.Black;

            string output = progress + " of " + total + " (" + string.Format("{0:F1}", pctComplete * 100) + "%)";
            Console.Write(output.PadRight(35) + stepDescription);
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
