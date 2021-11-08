using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDao
    {
        public Transfer SendTransactionScope(Transfer transfer);

        public Transfer GetTransfersByTransferId(int transferId);

        public Transfer UpdateTransactionScope(Transfer transfer);

        public List<Transfer> GetTransfersByUserId(int userId);

        public Transfer MakeTransferRequest(Transfer requestTransfer);

        public Transfer UpdateTransfer(Transfer transfer);
    }
}
