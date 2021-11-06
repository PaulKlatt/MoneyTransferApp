﻿using System;
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
        public int UserId { get; set; }
        public decimal Balance { get; set; }
    }

}
