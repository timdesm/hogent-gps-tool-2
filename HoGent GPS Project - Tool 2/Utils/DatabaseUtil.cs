using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoGent_GPS_Project___Tool_2
{
    class DatabaseUtil
    {
        public MySqlConnection connection;
        private String cs;

        public DatabaseUtil(String host, String user, String password, String database)
        {
            this.cs = @"server=" + host + ";userid=" + user + ";password=" + password + ";database=" + database;
        }

        public MySqlConnection getConnection()
        {
            return new MySqlConnection(this.cs);
        }

        public static MySqlCommand CommandExecutor(MySqlConnection cc, String sql)
        {
            return new MySqlCommand(sql, cc);
        }
    }
}
