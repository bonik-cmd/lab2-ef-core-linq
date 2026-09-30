using System;
using System.Collections.Generic;

namespace Lab2.Models;

public partial class Indicator
{
    public int IndicatorId { get; set; }

    public string Name { get; set; } = null!;

    public string? Unit { get; set; }

    public string? ReferenceRange { get; set; }

    public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();

    public virtual ICollection<AnalysisType> AnalysisTypes { get; set; } = new List<AnalysisType>();
}
