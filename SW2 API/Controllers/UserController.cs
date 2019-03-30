using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using sw2API.Data;
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
            return _dataContext.Users.Select(u => new {u.Id , u.FirstName, u.LastName, u.UserName, u.PhoneNumber }).ToList();

        }

        [HttpPost]
        public async Task<IActionResult> EditUser([FromBody] ApplicationUser model)
        {
            //Get User by the Email passed in.
            /*It's better practice to find user by the Id, (without exposing the id to the view).
            However, to keep this example simple, we can find the user by email instead*/
            var user = await _userManager.FindByEmailAsync(model.Id);
            //edit user: replace values of UserViewModel properties 
            user.UserName = model.UserName;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            //add user to the datacontext (database) and save changes
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok(new {user.UserName});
            }
            else
            {
                return BadRequest(new {result.Errors});
            }

            //this could be
            //return RedirectToAction("Index", "Home");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromBody] ApplicationUser model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user != null)
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