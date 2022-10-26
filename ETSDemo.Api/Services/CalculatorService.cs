using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Text.RegularExpressions;

namespace ETSDemo.Api.Services
{
    public class CalculatorService : ICalculatorService
    {
        public async Task<double> Calculate(string expression)
        {
            var pattern = @"(?<n>(\d)+(\.\d+)*)";
            var expr = Regex.Replace(expression, pattern, "(double)(${n})");
            return await CSharpScript.EvaluateAsync<double>(expr).ConfigureAwait(false);
        }
    }
}
