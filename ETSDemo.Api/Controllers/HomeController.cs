using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ETSDemo.Api.Models;
using ETSDemo.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ETSDemo.Api.Controllers
{
    [ApiController]
    [Route("loaderio-7a62593b48aa03b24119159a266d2c88.txt")]
    public class HomeController : ControllerBase
    {


        public HomeController()
        {
        }

        [HttpGet("")]
        public IActionResult Verify()
        {
            var content = System.IO.File.ReadAllText("./loaderio-7a62593b48aa03b24119159a266d2c88.txt");
            return Ok(content);
        }
    }
}
