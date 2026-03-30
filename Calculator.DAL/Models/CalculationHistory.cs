using System;
using System.Collections.Generic;

namespace Calculator.DAL.Models;

public partial class CalculationHistory
{
    public int CalculationId { get; set; }

    public string Expression { get; set; }

    public double Result { get; set; }

    public string OperationType { get; set; }

    public DateTimeOffset? CreatedAt { get; set; }
}
