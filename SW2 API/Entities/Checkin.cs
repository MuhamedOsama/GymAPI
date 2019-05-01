using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sw2API.Entities
{
    public class Checkin
    {
        public int Id { get; set; }
        public DateTime CheckinDate { get; set; }
        public int CustomerId { get; set; }
    }
}
