using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using sw2API.Entities;

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
        //[Required]
       // public string CustomerPicture { get; set; } // just a string of the file name
        public MembershipType MembershipType { get; set; }
        [ForeignKey("MembershipType")]
        public int MembershipTypeId { get; set; }
        public DateTime MembershipStart { get; set; }
        public DateTime MembershipEnd { get; set; }
        public int DaysLeft { get; set; }
    }
}