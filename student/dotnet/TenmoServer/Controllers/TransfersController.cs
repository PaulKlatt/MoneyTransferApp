using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;
using System.Transactions;

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

        [HttpPost("send")]
        //[Authorize]
        public ActionResult<Transfer> SendTransactionScope(Transfer sentTransfer)
        {
            // How to do transactions in c#?
            Transfer newTransfer = transferDao.SendTransactionScope(sentTransfer);
            return Created($"/transfers/{newTransfer.TransferId}", newTransfer);
        }
    }
}
