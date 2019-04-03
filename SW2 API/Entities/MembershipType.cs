using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sw2API.Entities
{
    public class MembershipType
    {
        public int MembershipTypeId { get; set; }
        public string Name { get; set; }
        public int SignUpFee { get; set; }
        public byte DurationInMonths { get; set; }
        public bool SaunaAccess { get; set; }
        public bool CrossFitAccess { get; set; }
    }
    
}
