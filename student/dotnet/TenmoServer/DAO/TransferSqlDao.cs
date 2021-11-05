using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferSqlDao : ITransferDao
    {
        private readonly string connectionString;
        public TransferSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public Transfer SendTransactionScope(Transfer transfer)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        SqlCommand command = new SqlCommand("UPDATE dbo.accounts SET balance = balance - @amount " +
                                                            "WHERE account_id = @fromAccount " +
                                                            "UPDATE dbo.accounts SET balance = balance + @amount " +
                                                            "WHERE account_id = @toAccount " +
                                                            "INSERT INTO dbo.transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) " +
                                                            "OUTPUT INSERTED.transfer_id " +
                                                            "VALUES (2, 2, @fromAccount, @toAccount, @amount)", connection);
                        command.Parameters.AddWithValue("@fromAccount", transfer.FromAccount);
                        command.Parameters.AddWithValue("@toAccount", transfer.ToAccount);
                        command.Parameters.AddWithValue("@amount", transfer.Amount);

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            transfer = GetTransferFromReader(reader);
                        }
                    }
                    scope.Complete();
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            return transfer;
        }

        private Transfer GetTransferFromReader(SqlDataReader reader)
        {
            Transfer transfer = new Transfer();
            transfer.TransferId = Convert.ToInt32(reader["transfer_id"]);
            transfer.TransferStatusId = Convert.ToInt32(reader["transfer_status_id"]);
            transfer.TransferTypeId = Convert.ToInt32(reader["transfer_type_id"]);
            transfer.Amount = Convert.ToDecimal(reader["amount"]);
            transfer.FromAccount = Convert.ToInt32(reader["account_from"]);
            transfer.ToAccount = Convert.ToInt32(reader["account_to"]);

            return transfer;
        }
    }
}
