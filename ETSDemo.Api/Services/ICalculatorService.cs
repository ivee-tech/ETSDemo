using System.Threading.Tasks;

namespace ETSDemo.Api.Services
{
    public interface ICalculatorService
    {
        Task<double> Calculate(string expression);
    }
}