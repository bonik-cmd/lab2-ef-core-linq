using System;
using System.Collections.Generic;

namespace Lab2.Models;

public partial class LabTechnician
{
    public int TechnicianId { get; set; }

    public string FullName { get; set; } = null!;

    public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
}
