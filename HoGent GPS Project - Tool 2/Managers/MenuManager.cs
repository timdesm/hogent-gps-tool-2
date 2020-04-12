using System;
using System.Collections.Generic;
using System.Text;

namespace HoGent_GPS_Project___Tool_2
{
    class MenuManager
    {

        public static void case1()
        {
            Boolean overviewLoop = true;
            while (overviewLoop)
            {
                Program.printHeader();
                Console.WriteLine("----- [DATABASE UPLOAD] -----");
                Console.WriteLine("");
                Console.Write("Are you sure to continue (Y/N)? ");
                String continueExport = Console.ReadLine();

                switch (continueExport.ToUpper())
                {
                    case "Y":
                        Program.printHeader();
                        Console.WriteLine("Generating data....");
                        DatabaseManager.uploadData(Program.data);
                        overviewLoop = false;
                        break;
                    case "N":
                        overviewLoop = false;
                        break;
                    default:
                        Console.Write("Wrong selection input, press ENTER to continue...");
                        Console.ReadLine();
                        break;
                }
            }
        }

        public static void case2()
        {

        }
    }
}
