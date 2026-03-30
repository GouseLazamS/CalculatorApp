using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Calculator.DAL.Models;

public partial class CalculationHistory
{
    public int CalculationId { get; set; }

    [JsonIgnore]
    public string Expression { get; set; }

    public double Result { get; set; }

    public string OperationType { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}
