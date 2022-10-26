using System.Threading.Tasks;

namespace ETSDemo.App.Services
{
    public interface ICalculatorService
    {
        Task<double> Calculate(string expression);
    }
}