using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sw2API.Models
{
    public class Customer
    {
        //Name/age/address/Contact phone/Emergency Phone
        public string FirstName { get; set; }
        //public string MiddleName { get; set; }
        public string LastName { get; set; }
        //[Required]
        public int Age { get; set; }
        public int Score { get; set; }
        //[Required]
        //public string Address { get; set; }
        //[Required]
        //public string PhoneNumber { get; set; }
        //public DateTime DOB { get; set; }
        public byte Gender { get; set; }
        //public string EmergencyPhoneNumbr { get; set; }

    }
}