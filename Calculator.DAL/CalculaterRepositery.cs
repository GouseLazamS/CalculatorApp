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

        
       public async Task<double> CalculateAsync(string expression, double n1, double n2, string operation)
        {
            // Perform Scientific Logic
            double result = operation.ToLower() switch
            {
                "add" => n1 + n2,
                "subtract" => n1 - n2,
                "multiply" => n1 * n2,
                "divide" => n2 != 0 ? n1 / n2 : throw new DivideByZeroException(),
                "pow" => Math.Pow(n1, n2),
                "sqrt" => n1 >= 0 ? Math.Sqrt(n1) : throw new ArgumentException("Negative Sqrt"),
                "sin" => Math.Sin(n1 * (Math.PI / 180)),
                "cos" => Math.Cos(n1 * (Math.PI / 180)),
                "log" => n1 > 0 ? Math.Log10(n1) : throw new ArgumentException("Log > 0"),
                _ => throw new NotSupportedException("Op not found")
            }; // Ensure semicolon is here

            // High-Performance Transaction
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var history = new CalculationHistory
                {
                    Expression = expression,
                    Result = result,
                    OperationType = operation,
                    CreatedAt = DateTimeOffset.Now
                };

                _context.CalculationHistories.Add(history);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task ClearHistoryAsync()
        {
            var history = await _context.CalculationHistories.ToListAsync();
            _context.CalculationHistories.RemoveRange(history);
            await _context.SaveChangesAsync();
        }
    }
}