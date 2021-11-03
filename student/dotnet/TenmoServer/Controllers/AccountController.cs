using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        // private readonly account dao

        [Authorize]
        [HttpGet]
        public decimal GetBalance(decimal userId)
        {
            //call the dao.get, refer to account table, get balance

            return 0;
        }


    }
}
