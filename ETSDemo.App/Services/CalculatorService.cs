using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Text.RegularExpressions;
using Microsoft.ApplicationInsights;

namespace ETSDemo.App.Services
{
    public class CalculatorService : ICalculatorService
    {

        private readonly TelemetryClient _telemetryClient;
        public CalculatorService(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient;
        }

        public async Task<double> Calculate(string expression)
        {
            var pattern = @"(?<n>(\d)+(\.\d+)*)";
            var expr = Regex.Replace(expression, pattern, "(double)(${n})");
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            var success = true;

            var result = await CSharpScript.EvaluateAsync<double>(expr);
            timer.Stop();
            _telemetryClient.TrackDependency(typeof(CSharpScript).FullName, nameof(CSharpScript.EvaluateAsync), expr, startTime, timer.Elapsed, success);
            return result;
        }
    }
}
