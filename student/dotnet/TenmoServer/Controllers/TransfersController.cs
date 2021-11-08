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
    [Authorize]
    [ApiController]
    public class TransfersController : Controller
    {

        private readonly ITransferDao transferDao;
        public TransfersController(ITransferDao _transferDao)
        {
            transferDao = _transferDao;
        }

        [HttpPost("send")]
        public ActionResult<Transfer> SendTransactionScope(Transfer sentTransfer)
        {
            Transfer newTransfer = transferDao.SendTransactionScope(sentTransfer);
            return Created($"/transfers/{newTransfer.TransferId}", newTransfer);
        }
        [HttpPut("approve")]
        public ActionResult<Transfer> UpdateTransactionScope(Transfer sentTransfer)
        {
            // Just realized we might want a transferdao.gettransferbyid just so we can double check this transfer exists
            Transfer newTransfer = transferDao.UpdateTransactionScope(sentTransfer);

            return Ok(newTransfer);
        }

        [HttpPost("request")]
        public ActionResult<Transfer> MakeTransferRequest(Transfer requestTransfer)
        {
            Transfer newTransfer = transferDao.MakeTransferRequest(requestTransfer);
            return Created($"/transfers/{newTransfer.TransferId}", newTransfer);
        }

        [HttpGet("users/{userId}")]
        public ActionResult<List<Transfer>> GetTransfersByUserId(int userId)
        {

            List<Transfer> myTransfers = transferDao.GetTransfersByUserId(userId);
            return Ok(myTransfers);
        }
    }
}
