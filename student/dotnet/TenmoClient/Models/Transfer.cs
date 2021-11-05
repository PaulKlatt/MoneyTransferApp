using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
{
    public class Transfer
    {
        public int TransferId { get; set; }

        public int TransferTypeId { get; set; }

        public int TransferStatusId { get; set; }

        public int FromAccount { get; set; }

        public int ToAccount { get; set; }

        public decimal Amount { get; set; }
    }
}
