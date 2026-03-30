using Microsoft.EntityFrameworkCore;
using Calculator.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calculator.DAL
{
    // Fix: Changed 'Calculater' to 'Calculator'
    public class CalculatorRepository : ICalculatorRepository
    {
        private readonly CalculatorDbContext _context;

        // Fix: Now this matches the class name exactly
        public CalculatorRepository(CalculatorDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CalculationHistory>> GetHistoryAsync()
        {
            return await _context.CalculationHistories
                .OrderByDescending(x => x.CalculationId)
                .ToListAsync();
        }

        public async Task<CalculationHistory> CalculateAsync(string operation, double n1, double? n2 = null)
        {
            double result = operation.ToLower() switch
            {
                "add" => n1 + (n2 ?? 0),
                "subtract" => n1 - (n2 ?? 0),
                "multiply" => n1 * (n2 ?? 1),
                "divide" => (n2 == 0) ? throw new DivideByZeroException("Cannot divide by zero.") : n1 / n2.Value,
                "pow" => Math.Pow(n1, n2 ?? 1),
                "sqrt" => n1 < 0 ? throw new ArgumentException("Cannot square root negative.") : Math.Sqrt(n1),
                "sin" => Math.Sin(n1 * (Math.PI / 180)),
                "cos" => Math.Cos(n1 * (Math.PI / 180)),
                "tan" => Math.Tan(n1 * (Math.PI / 180)),
                "log" => n1 <= 0 ? throw new ArgumentException("Log must be > 0.") : Math.Log10(n1),
                _ => throw new KeyNotFoundException($"Operation {operation} not supported.")
            };

            var entry = new CalculationHistory
            {
                Expression = n2.HasValue ? $"{n1} {operation} {n2}" : $"{operation}({n1})",
                Result = result,
                OperationType = operation,
                CreatedAt = DateTimeOffset.Now
            };

            _context.CalculationHistories.Add(entry);
            await _context.SaveChangesAsync();
            return entry;
        }

        public async Task ClearHistoryAsync()
        {
            var history = await _context.CalculationHistories.ToListAsync();
            _context.CalculationHistories.RemoveRange(history);
            await _context.SaveChangesAsync();
        }
    }
}