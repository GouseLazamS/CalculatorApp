using Calculator.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.DAL
{
    public interface ICalculatorRepository
    {
        Task<IEnumerable<CalculationHistory>> GetHistoryAsync();
        Task<CalculationHistory> CalculateAsync(string operation, double n1, double? n2 = null);
        Task ClearHistoryAsync();
    }
}
