using System;
using System.Collections.Generic;

namespace Lab2.Models;

public partial class ViewTestResult
{
    public int ResultId { get; set; }

    public string SampleBarcode { get; set; } = null!;

    public string Indicator { get; set; } = null!;

    public string? Unit { get; set; }

    public string? ReferenceRange { get; set; }

    public string ResultValue { get; set; } = null!;

    public string? DeviationStatus { get; set; }

    public string Technician { get; set; } = null!;

    public DateTime CompletedAt { get; set; }
}
