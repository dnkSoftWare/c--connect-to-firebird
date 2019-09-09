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

            Sample1(connectionSettings);
            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }

        private static void Sample2(Dictionary<string, string> connectionSettings)
        {

        }

        private static void Sample1(Dictionary<string, string> connectionSettings)
        {
            using (FBird fb = new FBird(connectionSettings))
            {
                fb.conn.Open();
                try
                {
                    #region DataReder Sample
                    IDataReader dr = fb.Select("select * from USERS;");

                    while (dr.Read())
                    {
                        Console.WriteLine("ID:" + dr.GetValue(0).ToString() + " Name:" + dr.GetString(1));
                    }

                    dr.Close(); 
                    #endregion
                    Console.ReadLine();
                    #region Добавление 2-х записей в одной транзакции
                    FbTransaction tr = null;
                    var col = fb.Execute(@"
                INSERT INTO USERS(NAME, PSW, LEVEL, ONLINE, CURRVERSION, ROLE_NAME, USER_NAME, PSW_USER, IP) 
              VALUES('Каруна Т.А.', '222345', 1, 0, NULL, 'R', 'READER', '123', '192.168.2.40'); ", ref tr, false);

                    col += fb.Execute(@"
                INSERT INTO USERS(NAME, PSW, LEVEL, ONLINE, CURRVERSION, ROLE_NAME, USER_NAME, PSW_USER, IP) 
              VALUES('Каруна Т.П.', '123345', 1, 0, NULL, 'R', 'READER', '123', '192.168.2.40'); ", ref tr, true);

                    Console.WriteLine("Добавлено {0} записей!", col);

                    dr = fb.Select("select * from USERS;");

                    while (dr.Read())
                    {
                        Console.WriteLine("ID:" + dr.GetValue(0).ToString() + " Name:" + dr.GetString(1));
                    }

                    dr.Close();
                    #endregion
                }
                finally
                {
                    fb.conn.Close();
                    Console.ReadLine();
                }
            }
        }
    }
}
