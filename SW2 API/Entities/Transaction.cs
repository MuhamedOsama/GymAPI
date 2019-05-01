using sw2API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace sw2API.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int PayedAmount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
