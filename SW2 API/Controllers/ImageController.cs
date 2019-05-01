using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sw2API.Data;
using sw2API.Models;

namespace ImageUploadDemo.Controllers
{
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public static IHostingEnvironment _environment;
        public ImageController(IHostingEnvironment environment, ApplicationDbContext context)
        {
            _environment = environment;
            _context = context;
        }
        public class FIleUploadAPI
        {
            public IFormFile Files { get; set; }
        }

        [HttpGet("{fileName}")]
        public ActionResult<FIleUploadAPI> GetImage(string fileName)
        {
            string Filepath = _environment.WebRootPath + "\\uploads\\" + fileName;
            if (System.IO.File.Exists(Filepath))
            {
                return PhysicalFile(Filepath, "image/jpeg");
            }
            else
            {
                return NotFound("No image found");
            }


        }

        [HttpPost("{Id}")]
        public ActionResult Post(FIleUploadAPI files,[FromRoute] int Id)
        {
            Customer customer = _context.Customers.Find(Id);
            if (customer != null)
            {
                if (files.Files.Length > 0)
                {
                    try
                    {
                        if (!Directory.Exists(_environment.WebRootPath + "\\uploads\\"))
                        {
                            Directory.CreateDirectory(_environment.WebRootPath + "\\uploads\\");
                        }

                        string fileName = DateTime.Now.ToString("yyyyMMddTHHmmss") + files.Files.FileName;
                        using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + "\\uploads\\" + fileName))
                        {
                            files.Files.CopyTo(filestream);
                            filestream.Flush();
                            return Ok(new {path = "\\uploads\\" + files.Files.FileName});
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(new{message = ex.ToString()});
                    }
                }
                else
                {
                    return BadRequest(new{message =  "Unsuccessful"});
                }
            }
            else
            {
                return NotFound(new { message = "customer with this ID not found, can't upload file.."});
            }
            

        }
    }
}