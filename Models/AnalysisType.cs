using System;
using System.Collections.Generic;

namespace Lab2.Models;

public partial class AnalysisType
{
    public int AnalysisTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string TestCode { get; set; } = null!;

    public decimal Price { get; set; }

    public string BiomaterialType { get; set; } = null!;

    public int ExecutionDays { get; set; }

    public virtual ICollection<Sample> Samples { get; set; } = new List<Sample>();

    public virtual ICollection<Indicator> Indicators { get; set; } = new List<Indicator>();
}
