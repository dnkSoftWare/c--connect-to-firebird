using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;

namespace FireBirdSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> connectionSettings = new Dictionary<string, string>()
            {
                 {"User","SYSDBA"},
                 {"Password","masterkey"},
                 {"Database","F:\\Delphi\\Projects\\dnk-Учет-nb\\db\\UCHET_MS.GDB"},
                 {"DataSource","localhost"},
                 {"Port","3050"},
                 {"Dialect","3"},
                 {"Charset","WIN1251"},
                 {"Role",""},
                 {"Connection lifetime","15"},
                 {"Pooling","true"},
                 {"MinPoolSize","0"},
                 {"MaxPoolSize","50"},
                 {"Packet Size","8192"},
                 {"ServerType","0"}
            };
            FBird fb = new FBird(connectionSettings);

            //            Console.ReadKey(); 
            //            throw new ApplicationException("Exit"); 
            //
            //            string connectionString =
            //                "User=SYSDBA;" +
            //                "Password=masterkey;" +
            //                "Database=F:\\Delphi\\Projects\\dnk-Учет-nb\\db\\UCHET_MS.GDB;" +
            //                "DataSource=localhost;" +
            //                "Port=3050;" +
            //                "Dialect=3;" +
            //                "Charset=WIN1251;" +
            //                "Role=;" +
            //                "Connection lifetime=15;" +
            //                "Pooling=true;" +
            //                "MinPoolSize=0;" +
            //                "MaxPoolSize=50;" +
            //                "Packet Size=8192;" +
            //                "ServerType=0";
            //
            //            var myConnection1 = new FbConnection(connectionString);
            //            myConnection1.Open();

            fb.conn.Open();
            try
            {

               IDataReader dr = fb.Select("select * from USERS;");

                while (dr.Read())
                {
                  Console.WriteLine("ID:"+ dr.GetValue(0).ToString()+" Name:"+ dr.GetString(1));  
                }
                dr.Close();
            }
            finally
            {
                fb.conn.Close();
               Console.ReadLine();
            }
            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
