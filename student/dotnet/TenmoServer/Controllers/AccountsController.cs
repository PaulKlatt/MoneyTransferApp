using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class AccountsController : ControllerBase
    {

        // private readonly account dao
        private readonly IAccountDao accountDao;
        public AccountsController(IAccountDao _accountDao)
        {
            accountDao = _accountDao;
        }

        [HttpGet("{userId}")]
        public ActionResult<List<Account>> GetAccountsByUser(int userId)
        {
            List<Account> accounts = accountDao.GetAccountByAccountId(userId);
            if (accounts.Count == 0)
            {
                return NotFound();
            }
            // Maybe add check is balance is negative
            return Ok(accounts);
            // Maybe return full Account?
        }
    }
}
