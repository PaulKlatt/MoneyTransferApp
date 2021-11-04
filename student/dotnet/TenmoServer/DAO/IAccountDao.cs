using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
<<<<<<< HEAD

namespace TenmoServer.DAO
{
    interface IAccountDao
    {
=======
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IAccountDao
    {
        public List<Account> GetAccountByAccountId(int accountId);
>>>>>>> ee5e71762521729465f06b586ac35826ffd0cae3
    }
}
