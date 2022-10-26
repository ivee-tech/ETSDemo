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
    [Route("api/[controller]")]
    public class CalculatorController : ControllerBase
    {

        private readonly ILogger<CalculatorController> _logger;
        private readonly ICalculatorService _calcSvc;

        public CalculatorController(ILogger<CalculatorController> logger, ICalculatorService calcSvc)
        {
            _logger = logger;
            _calcSvc = calcSvc;
        }

        [HttpGet("result")]
        public async Task<IActionResult> Calculate(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return BadRequest("Expression is required.");
            }
            var expr = Uri.UnescapeDataString(expression);
            var result = await _calcSvc.Calculate(expr).ConfigureAwait(false);
            var model = new ResultModel()
            {
                Expression = expr,
                Result = result
            };
            return Ok(model);
        }

        [HttpPost("build")]
        public async Task<IActionResult> SendBuild(dynamic build)
        {
            var s = System.Text.Json.JsonSerializer.Serialize(build);
            dynamic obj = JsonConvert.DeserializeObject(s);
            if (obj == null)
            {
                return BadRequest("Build object is required.");
            }
            if (obj.id == null)
            {
                return BadRequest("Build id is required.");
            }

            var model = new { buildId = obj.id.ToString(), message = $"Build {obj.id} sent succesfully." };
            return Ok(model);
        }

        [HttpPost("wi")]
        public async Task<IActionResult> SendWorkItem(dynamic workItem)
        {
            var s = System.Text.Json.JsonSerializer.Serialize(workItem);
            dynamic obj = JsonConvert.DeserializeObject(s);
            if (obj == null)
            {
                return BadRequest("WorkItem object is required.");
            }
            if (obj.id == null)
            {
                return BadRequest("WorkItem id is required.");
            }

            var model = new { workItemId = obj.id.ToString(), message = $"Work Item {obj.id} sent succesfully." };
            return Ok(model);
        }
    }
}
