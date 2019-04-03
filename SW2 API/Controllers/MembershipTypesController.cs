using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sw2API.Data;
using sw2API.Entities;

namespace sw2API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MembershipTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/MembershipTypes
        [HttpGet]
        public IEnumerable<MembershipType> GetMembershipTypes()
        {
            return _context.MembershipTypes;
        }

        // GET: api/MembershipTypes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMembershipType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var membershipType = await _context.MembershipTypes.FindAsync(id);

            if (membershipType == null)
            {
                return NotFound();
            }

            return Ok(membershipType);
        }

        // PUT: api/MembershipTypes/5
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> PutMembershipType([FromRoute] int id, [FromBody] MembershipType membershipType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != membershipType.MembershipTypeId)
            {
                return BadRequest();
            }

            _context.Entry(membershipType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MembershipTypeExists(id))
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

        // POST: api/MembershipTypes
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> PostMembershipType([FromBody] MembershipType membershipType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MembershipTypes.Add(membershipType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMembershipType", new { id = membershipType.MembershipTypeId }, membershipType);
        }

        // DELETE: api/MembershipTypes/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteMembershipType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var membershipType = await _context.MembershipTypes.FindAsync(id);
            if (membershipType == null)
            {
                return NotFound();
            }

            _context.MembershipTypes.Remove(membershipType);
            await _context.SaveChangesAsync();

            return Ok(membershipType);
        }

        private bool MembershipTypeExists(int id)
        {
            return _context.MembershipTypes.Any(e => e.MembershipTypeId == id);
        }
    }
}