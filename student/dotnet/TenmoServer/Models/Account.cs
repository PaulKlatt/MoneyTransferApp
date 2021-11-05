using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace TenmoServer.Models
{
    public class Account
    {
        public int? AccountId { get; set; }

        [Required(ErrorMessage = "User ID field is required.")]
        public int UserId { get; set; }

        [Range((double)0.00M, double.PositiveInfinity, ErrorMessage = "Balance is required.")]
        public decimal Balance { get; set; }
    }
}
