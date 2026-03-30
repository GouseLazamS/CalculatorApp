using Calculator.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using Calculator.DAL.Models;

namespace Calculator.DAL
{
    public interface ICalculatorRepository
    {
        Task<IEnumerable<CalculationHistory>> GetHistoryAsync();
        // Ensure exactly 4 parameters are here
        Task<double> CalculateAsync(string expression, double n1, double n2, string operation);
        Task ClearHistoryAsync();
    }
}
