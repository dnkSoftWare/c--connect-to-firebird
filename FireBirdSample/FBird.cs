using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using FirebirdSql.Data.FirebirdClient;

namespace FireBirdSample
{
    public class FBird
    {
        private StringBuilder connString;
        private FbTransaction write_transaction;
        private FbTransaction read_transaction;
        public FbConnection conn { get; set; }
        public FBird(Dictionary<string, string> connectionSettings)
        {
            connString = new StringBuilder("");
            foreach (KeyValuePair<string, string> pair in connectionSettings)
            {
                FbConnectionStringBuilder.AppendKeyValuePair(connString, pair.Key, pair.Value);
            }
            conn = new FbConnection(connString.ToString());
            
        }

        private FbTransaction GetTransaction(bool readable = true)
        {
            if (readable)
            {
                var tro = new FbTransactionOptions()
                {
                    TransactionBehavior = FbTransactionBehavior.Read | FbTransactionBehavior.ReadCommitted | FbTransactionBehavior.RecVersion
                };

                read_transaction = conn.BeginTransaction(tro);
                return read_transaction;
            }
            else
            {
                var tro = new FbTransactionOptions()
                {
                    TransactionBehavior = FbTransactionBehavior.NoWait | FbTransactionBehavior.ReadCommitted | FbTransactionBehavior.RecVersion
                };

                write_transaction = conn.BeginTransaction(tro);
                return write_transaction;

            }
        }

        public FbDataReader Select(string sql)
        {
            

            var cmd = new FbCommand(sql, conn, GetTransaction());
            
            return cmd.ExecuteReader(); // DataReader
        }
    }
}