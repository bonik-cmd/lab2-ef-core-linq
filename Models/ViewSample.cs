using System;
using System.Collections.Generic;

namespace Lab2.Models;

public partial class ViewSample
{
    public int SampleId { get; set; }

    public string Barcode { get; set; } = null!;

    public string Patient { get; set; } = null!;

    public string AnalysisType { get; set; } = null!;

    public string Biomaterial { get; set; } = null!;

    public decimal Price { get; set; }

    public DateTime CollectedAt { get; set; }

    public string Status { get; set; } = null!;
}
