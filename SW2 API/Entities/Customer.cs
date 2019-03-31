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
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public int Age { get; set; }
        public string Address { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public DateTime DOB { get; set; }
        [Required]
        public byte Gender { get; set; }
        public string EmergencyPhoneNumbr { get; set; }

    }
}