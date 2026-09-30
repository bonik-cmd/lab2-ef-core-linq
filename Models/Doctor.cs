using System;
using System.Collections.Generic;

namespace Lab2.Models;

public partial class Doctor
{
    public int DoctorId { get; set; }

    public string FullName { get; set; } = null!;

    public string Specialty { get; set; } = null!;

    public string MedicalOrgName { get; set; } = null!;

    public string? Phone { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
