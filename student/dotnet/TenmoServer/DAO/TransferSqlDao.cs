using Microsoft.AspNetCore.Mvc;
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
            Transfer newTransfer = new Transfer();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        SqlCommand command = new SqlCommand("UPDATE dbo.accounts SET balance = balance - @amount " +
                                                            "WHERE account_id = @accountFrom " +
                                                            "UPDATE dbo.accounts SET balance = balance + @amount " +
                                                            "WHERE account_id = @accountTo " +
                                                            "INSERT INTO dbo.transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) " +
                                                            "OUTPUT INSERTED.transfer_id " +
                                                            "VALUES (2, 2, @accountFrom, @accountTo, @amount)", connection);
                        command.Parameters.AddWithValue("@accountFrom", transfer.AccountFrom);
                        command.Parameters.AddWithValue("@accountTo", transfer.AccountTo);
                        command.Parameters.AddWithValue("@amount", transfer.Amount);

                        int newId = (int)command.ExecuteScalar();

                        SqlCommand command2 = new SqlCommand("SELECT transfer_id, transfer_type_id, transfer_status_id, account_from, account_to, amount " +
                                                            "FROM dbo.transfers " +
                                                            "WHERE transfer_id = @newId", connection);
                        command2.Parameters.AddWithValue("@newId", newId);

                        SqlDataReader reader = command2.ExecuteReader();
                        if(reader.Read())
                        {
                           newTransfer = GetTransferFromReader(reader);
                        }
                    }
                    scope.Complete();
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            return newTransfer;
        }

        public List<Transfer> GetTransfersByUserId(int userId)
        {
            List<Transfer> myTransfers = new List<Transfer>();
            try 
            {            
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("SELECT transfer_id, transfer_type_id, transfer_status_id, account_from, account_to, amount " +
                                                        "FROM dbo.transfers " +
                                                        "JOIN dbo.accounts " +
                                                        "ON dbo.transfers.account_from = dbo.accounts.account_id OR dbo.transfers.account_to = dbo.accounts.account_id " +
                                                        "WHERE user_id = @userId", connection);
                    command.Parameters.AddWithValue("@userId", userId);


                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        myTransfers.Add(GetTransferFromReader(reader));
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return myTransfers;
        }

        private Transfer GetTransferFromReader(SqlDataReader reader)
        {
            Transfer transfer = new Transfer();
            transfer.TransferId = Convert.ToInt32(reader["transfer_Id"]);
            transfer.TransferStatusId = Convert.ToInt32(reader["transfer_status_id"]);
            transfer.TransferTypeId = Convert.ToInt32(reader["transfer_type_id"]);
            transfer.Amount = Convert.ToDecimal(reader["amount"]);
            transfer.AccountFrom = Convert.ToInt32(reader["account_from"]);
            transfer.AccountTo = Convert.ToInt32(reader["account_to"]);

            return transfer;
        }
    }
}
