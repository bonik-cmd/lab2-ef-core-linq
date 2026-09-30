using System;
using System.Collections.Generic;

namespace Lab2.Models;

public partial class ViewOrder
{
    public int OrderId { get; set; }

    public string Patient { get; set; } = null!;

    public string Doctor { get; set; } = null!;

    public string DoctorSpecialty { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string PaymentStatus { get; set; } = null!;

    public string OverallStatus { get; set; } = null!;
}
