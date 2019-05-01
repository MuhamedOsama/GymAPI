using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sw2API.Data;
using sw2API.Entities;
using sw2API.Models;

namespace sw2API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public CustomersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Customers
        [HttpGet]
        public IEnumerable<Customer> GetCustomers()
        {
            return _context.Customers;
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _context.Customers.Include("MembershipType").FirstOrDefaultAsync(x => x.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }
        //PUT: api/customers/CheckIn/5
        [HttpPut("CheckIn/{id}")]
        public async Task<IActionResult> CheckInCustomer([FromRoute] int id)
        {
           Customer customer = await _context.Customers.FindAsync(id);
           if (customer != null && customer.DaysLeft>0)
           {
               customer.DaysLeft--;
               var result = _context.SaveChanges();
               return Ok(new { customer.DaysLeft });
            }
           else
           {
               return BadRequest("No customer with that id was found or Customer has no more days left");
           }
             
        }
        //PUT: api/customers/RenewMembership/id
        [HttpPut("RenewMembership/{id}")]
        public async Task<IActionResult> RenewMembership([FromRoute] int id)
        {
            
            Customer customer = await _context.Customers.Include("MembershipType").FirstOrDefaultAsync(x => x.Id == id);
            if (customer != null && customer.DaysLeft == 0)
            {
                customer.DaysLeft = customer.MembershipType.DurationInMonths * 30;
                _context.SaveChanges();
                Transaction transaction = new Transaction
                {
                    CustomerId = customer.Id,
                    PayedAmount = customer.MembershipType.SignUpFee,
                    TransactionDate = DateTime.Now
                };
                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();
                return Ok(new {customer.DaysLeft});
            }
            else
            {
                if (customer==null)
                {
                    return NotFound(new {message = "Customer with that id was not found"});
                }
                else
                {
                    return BadRequest(new { message = "customer membership hasn't ended yet"});
                } 
            }
        }
        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer([FromRoute] int id, [FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.Id)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<IActionResult> PostCustomer([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            //customer.CustomerPicture = DateTime.Now.ToString("yyyyMMddTHHmmss") + customer.CustomerPicture;
            MembershipType membership = await _context.MembershipTypes.FindAsync(customer.MembershipTypeId);
            customer.MembershipStart = DateTime.Today;
            customer.MembershipEnd = customer.MembershipStart.AddMonths(membership.DurationInMonths);
            customer.DaysLeft = membership.DurationInMonths * 30;
            _context.Customers.Add(customer);
            _context.SaveChanges();
            Transaction transaction = new Transaction
            {
                CustomerId = customer.Id,
                PayedAmount = membership.SignUpFee,
                TransactionDate = DateTime.Now
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetCustomer", new { id = customer.Id }, transaction);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return Ok(customer);
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}