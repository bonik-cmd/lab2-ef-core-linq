using System;
using System.Collections.Generic;

namespace Lab2.Models;

public partial class TestResult
{
    public int ResultId { get; set; }

    public int SampleId { get; set; }

    public int IndicatorId { get; set; }

    public int TechnicianId { get; set; }

    public DateTime CompletedAt { get; set; }

    public string ResultValue { get; set; } = null!;

    public string? DeviationStatus { get; set; }

    public virtual Indicator Indicator { get; set; } = null!;

    public virtual Sample Sample { get; set; } = null!;

    public virtual LabTechnician Technician { get; set; } = null!;
}
