using System;
using System.Collections.Generic;

namespace Lab2.Models;

public partial class Sample
{
    public int SampleId { get; set; }

    public string Barcode { get; set; } = null!;

    public int OrderId { get; set; }

    public int AnalysisTypeId { get; set; }

    public DateTime CollectedAt { get; set; }

    public string Status { get; set; } = null!;

    public virtual AnalysisType AnalysisType { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
}
