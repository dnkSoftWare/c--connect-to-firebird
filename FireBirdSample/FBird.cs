using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using FirebirdSql.Data.FirebirdClient;

namespace FireBirdSample
{
    public class FBird : IDisposable
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
                if (write_transaction != null)
                {
                    // if(conn.)
                    write_transaction.RollbackRetaining();
                    write_transaction.Dispose();
                }

                var tro = new FbTransactionOptions()
                {
                    TransactionBehavior = FbTransactionBehavior.Read | FbTransactionBehavior.ReadCommitted | FbTransactionBehavior.RecVersion
                };

                read_transaction = conn.BeginTransaction(tro);
                return read_transaction;
            }
            else
            {
                if (read_transaction != null)
                {
                    read_transaction.CommitRetaining();
                    read_transaction.Dispose();
                }

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
            
            var cmd = new FbCommand(sql, conn , GetTransaction());
            
            return cmd.ExecuteReader(); // DataReader
        }

        public int Execute(string sql, ref FbTransaction tr, bool commit_auto = true)
        {
            if (tr == null)
             tr = GetTransaction(false);

            var cmd = new FbCommand(sql, conn, tr);
            var rowAffected = cmd.ExecuteNonQuery();

            if (commit_auto)
            {
                tr.CommitRetaining();
            }

            return rowAffected;

        }

        public void Dispose()
        {
            conn.Close();
        }
    }
}