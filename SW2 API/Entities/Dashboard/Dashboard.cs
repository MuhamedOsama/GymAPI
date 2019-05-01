using sw2API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sw2API.Entities
{
    public class Dashboard
    {
        public Dashboard()
        {
            MonthlyEarnings = new List<Earning>();
            Subscriptions = new List<Subscription>();
            DayCheckins = new List<DayCheckin>();
        }
        public int TotalCustomers { get; set; }
        public int TotalCheckinsToday { get; set; }
        public int EarningsToday { get; set; }
        public int TotalEarnings { get; set; }
        public List<DayCheckin> DayCheckins { get; set; }
        public List<Earning> MonthlyEarnings { get; set; }
        public List<Subscription> Subscriptions { get; set; }
    }
}
