using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using sw2API.Data;
using sw2API.Entities;
using sw2API.Models;

namespace sw2API.Controllers
{
    [Route("User")]
    [Authorize(Policy = "AdminOnly")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _dataContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(ApplicationDbContext dataContext, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            // Inject the datacontext and userManager Dependencies
            _dataContext = dataContext;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Object>> Get()
        {
            return _dataContext.Users.Select(u => new {u.Id , u.FirstName, u.LastName, u.UserName, u.PhoneNumber }).Where(u=>u.UserName!="admin").ToList();

        }
        [HttpGet]
        [Route("DashboardData")]
        public ActionResult<IActionResult> DashboardData()
        {
            DateTime today = DateTime.Today;
            Dashboard dashboard = new Dashboard
            {
                TotalCustomers = _dataContext.Customers.Count(),
                TotalEarnings = _dataContext.Transactions.Sum(m => m.PayedAmount),
                EarningsToday = _dataContext.Transactions.Where(m=>m.TransactionDate.ToString("D") == today.ToString("D")).Sum(m=>m.PayedAmount),
                TotalCheckinsToday = _dataContext.Checkins.Where(m => m.CheckinDate.ToString("D") == today.ToString("D")).Count()
            };
            List<string> MonthsNames = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.ToList();
            List<string> DaysNames = CultureInfo.CurrentCulture.DateTimeFormat.DayNames.ToList();
            IQueryable<Checkin> CheckinsForThisMonth = _dataContext.Checkins.Where(m => m.CheckinDate.Month == today.Month);
            DaysNames.ForEach(day =>
            {   
                dashboard.DayCheckins.Add(new DayCheckin
                {
                    Day = day,
                    TotalCheckins = CheckinsForThisMonth.Where( m => CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(m.CheckinDate.DayOfWeek) == day).Count()
                });
            });
            IQueryable<Transaction> EarningsThisYear = _dataContext.Transactions.Where(m => m.TransactionDate.Year == today.Year);
            MonthsNames.ForEach(month =>
            {
                if (month != "")
                {
                    dashboard.MonthlyEarnings.Add(new Earning
                    {
                        Month = month,
                        Amount = EarningsThisYear.Where(m => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m.TransactionDate.Month) == month).Sum(m => m.PayedAmount)
                    });
                }

            });

            dashboard.EarningsToday = _dataContext.Transactions.Where(m => m.TransactionDate.ToString("D") == today.ToString("D")).Sum(m => m.PayedAmount);
            var Memberships = _dataContext.MembershipTypes.ToList();
            Memberships.ForEach(membership =>
            {
                Subscription s = new Subscription
                {
                    Name = membership.Name,
                    TotalSubscribers = _dataContext.Customers.Where(m => m.MembershipTypeId == membership.MembershipTypeId).Count()

                };
                dashboard.Subscriptions.Add(s);
            });

            return Ok(dashboard);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditUser([FromRoute] string id,[FromBody] ApplicationUser model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }
            var user = await _userManager.FindByIdAsync(model.Id);
            user.UserName = model.UserName;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok(new {user.UserName});
            }
            else
            {
                return BadRequest(new {result.Errors});
            }

            
        }
        //register cashier
        [HttpPost]
        [Route("registerCashier")]
        public async Task<ActionResult> CreateUser([FromBody] RegisterCashierModel model)
        {
            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                PhoneNumber = model.PhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Cashier");
                return Ok(new { Username = user.UserName });
            }
            else
            {
                return BadRequest(new { result.Errors });
            }

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromBody] ApplicationUser model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user != null || user.UserName!="admin")
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return Ok(new { user });
                }
                else
                {
                    return BadRequest(new { result.Errors });
                }
            }
            else
            {
                return NotFound();
            }
            
            
        }
    }
}