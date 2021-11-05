using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Transfer
    {
        public int? TransferId { get; set; }

        [Required]
        public int TransferTypeId { get; set; }
        [Required]
        public int TransferStatusId { get; set; }
        [Required]
        public int FromAccount { get; set; }
        [Required]
        public int ToAccount { get; set; }
        // Assuming it follows money convention
        [Range((double)0.01, double.PositiveInfinity)]
        public decimal Amount { get; set; }
    }
}
