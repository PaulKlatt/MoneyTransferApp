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
    [ApiController]
    public class TransfersController : Controller
    {

        private readonly ITransferDao transferDao;
        public TransfersController(ITransferDao _transferDao)
        {
            transferDao = _transferDao;
        }

        [HttpPost]
        //[Authorize]
        public Transfer CreateSendTransaction(Transfer newTransfer)
        {
            // How to do transactions in c#?

            return null;
        }
    }
}
