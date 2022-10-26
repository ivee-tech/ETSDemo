using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ETSDemo.App.Models;
using ETSDemo.App.Services;
using System.Net.Http;
using System.Net;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace ETSDemo.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICalculatorService _calcSvc;
        private readonly TelemetryClient _telemetryClient;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, ICalculatorService calcSvc, TelemetryClient telemetryClient, IMemoryCache cache, IConfiguration configuration)
        {
            _logger = logger;
            _calcSvc = calcSvc;
            _telemetryClient = telemetryClient;
            _cache = cache;
            _configuration = configuration;
            ViewData["AppInsightsConnectionString"] = _configuration["ApplicationInsights:ConnectionString"];
        }

        public IActionResult Index()
        {
            if (_configuration["EnableCustomEvents"] == "Y")
            {
                _telemetryClient.TrackEvent("homePageRequested");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Calculator()
        {
            return View();
        }

        public async Task<IActionResult> Calculate(string expression)
        {
            if(string.IsNullOrEmpty(expression))
            {
                var message = "Expression is required.";
                _telemetryClient.TrackTrace("Expression is required.");
                return BadRequest(message);
            }
            var expr = Uri.UnescapeDataString(expression);
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            var success = true;

            try
            {
                //string s1 = null;
                //if (s1 == null)
                //{
                //    _telemetryClient.TrackTrace("S1 is null", Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Error);
                //}
                //else
                //{
                //    var s2 = s1.Substring(0);
                //}
                var result = await _calcSvc.Calculate(expr);
                var value = 1;
                _telemetryClient.GetMetric("calcRequests").TrackValue(value);
                _telemetryClient.GetMetric("results").TrackValue(result);
                return Json(result);
            }
            catch(Exception ex)
            {
                success = false;
                _telemetryClient.TrackException(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
            finally
            {
                timer.Stop();
                _telemetryClient.TrackDependency(_calcSvc.GetType().FullName, nameof(_calcSvc.Calculate), expr, startTime, timer.Elapsed, success);
            }
        }
        public IActionResult GetVisitorCount()
        {
            var visitorCount = 0;
            _cache.TryGetValue(Constants.VisitorCountKey, out visitorCount);
            return Json(visitorCount);
        }
    }
}
