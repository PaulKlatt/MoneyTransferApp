using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        // private readonly account dao
        private readonly IAccountDao accountDao;
        public AccountController(IAccountDao _accountDao)
        {
            accountDao = _accountDao;
        }

        [Authorize]
        [HttpGet("{accountId}")]
        public decimal GetBalance(int accountId)
        {

            //call the dao.get, refer to account table, get balance
            return accountDao.GetBalance(accountId);
            
        }


    }
}
