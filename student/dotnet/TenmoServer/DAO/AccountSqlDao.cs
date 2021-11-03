using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.DAO
{
    public class AccountSqlDao : IAccountDao
    {
        private readonly string connectionString;

        public AccountSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public decimal GetBalance()
        {
            return 0;
        }
    }
}
