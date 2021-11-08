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
        // pretty these posts and the puts are supposed to be combined to be truly restful
        [HttpPost("send")]
        public ActionResult<Transfer> SendTransactionScope(Transfer sentTransfer)
        {
            Transfer newTransfer = transferDao.SendTransactionScope(sentTransfer);
            return Created($"/transfers/{newTransfer.TransferId}", newTransfer);
        }
        [HttpPut("approve")]
        public ActionResult<Transfer> UpdateTransactionScope(Transfer sentTransfer)
        {
            Transfer transferToUpdate = transferDao.GetTransfersByTransferId((int)(sentTransfer.TransferId));
            if (transferToUpdate == null)
            {
                return NotFound("Transfer does not exist");
            }
            else
            {
                Transfer newTransfer = transferDao.UpdateTransactionScope(sentTransfer);
                return Ok(newTransfer);
            }
        }

        [HttpPut("reject")]
        public ActionResult<Transfer> UpdateTransfer(Transfer sentTransfer)
        {
            Transfer transferToUpdate = transferDao.GetTransfersByTransferId((int)(sentTransfer.TransferId));
            if (transferToUpdate == null)
            {
                return NotFound("Transfer does not exist");
            }
            else
            {
                Transfer newTransfer = transferDao.UpdateTransfer(sentTransfer);
                return Ok(newTransfer);
            }
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

        [HttpGet("{transferId}")]
        public ActionResult<Transfer> GetTransferByTransferId(int transferId)
        {
            Transfer newTransfer = transferDao.GetTransfersByTransferId(transferId);
            if (newTransfer == null)
            {
                return NotFound("No transfer found for this ID.");
            }
            else
            {
                return Ok(newTransfer);
            }
        }
    }
}
