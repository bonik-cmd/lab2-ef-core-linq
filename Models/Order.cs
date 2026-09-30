using System;
using System.Collections.Generic;

namespace Lab2.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int PatientId { get; set; }

    public int DoctorId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string PaymentStatus { get; set; } = null!;

    public string OverallStatus { get; set; } = null!;

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;

    public virtual ICollection<Sample> Samples { get; set; } = new List<Sample>();
}
