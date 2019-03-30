using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sw2API.Entities;

namespace SW2_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        [EnableQuery()]
        public ActionResult<IEnumerable<Customer>> Get()
        {
            return new List<Customer>
            {
                new Customer {FirstName = "Mohamed", LastName = "Osama", Age = 20, Gender = 1, Score = 65},
                new Customer {FirstName = "Salma", LastName = "Osama", Age = 22, Gender = 2, Score = 90},
                new Customer {FirstName = "Mohamed", LastName = "Hossam", Age = 19, Gender = 1, Score = 78}
            };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
