using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Lab2.Models;

public partial class ClinicalLaboratoryDbContext : DbContext
{
    public ClinicalLaboratoryDbContext(DbContextOptions<ClinicalLaboratoryDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AnalysisType> AnalysisTypes { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Indicator> Indicators { get; set; }

    public virtual DbSet<LabTechnician> LabTechnicians { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Sample> Samples { get; set; }

    public virtual DbSet<TestResult> TestResults { get; set; }

    public virtual DbSet<ViewOrder> ViewOrders { get; set; }

    public virtual DbSet<ViewSample> ViewSamples { get; set; }

    public virtual DbSet<ViewTestResult> ViewTestResults { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AnalysisType>(entity =>
        {
            entity.HasKey(e => e.AnalysisTypeId).HasName("PK_AnalysisTypes");

            entity.ToTable("ANALYSIS_TYPES");

            entity.HasIndex(e => e.TestCode, "UQ_AnalysisTypes_TestCode").IsUnique();

            entity.Property(e => e.AnalysisTypeId).HasColumnName("analysis_type_id");
            entity.Property(e => e.BiomaterialType)
                .HasMaxLength(100)
                .HasColumnName("biomaterial_type");
            entity.Property(e => e.ExecutionDays).HasColumnName("execution_days");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.TestCode)
                .HasMaxLength(50)
                .HasColumnName("test_code");

            entity.HasMany(d => d.Indicators).WithMany(p => p.AnalysisTypes)
                .UsingEntity<Dictionary<string, object>>(
                    "AnalysisTypeIndicator",
                    r => r.HasOne<Indicator>().WithMany()
                        .HasForeignKey("IndicatorId")
                        .HasConstraintName("FK_ATI_Indicators"),
                    l => l.HasOne<AnalysisType>().WithMany()
                        .HasForeignKey("AnalysisTypeId")
                        .HasConstraintName("FK_ATI_AnalysisTypes"),
                    j =>
                    {
                        j.HasKey("AnalysisTypeId", "IndicatorId").HasName("PK_AnalysisTypeIndicators");
                        j.ToTable("ANALYSIS_TYPE_INDICATORS");
                        j.IndexerProperty<int>("AnalysisTypeId").HasColumnName("analysis_type_id");
                        j.IndexerProperty<int>("IndicatorId").HasColumnName("indicator_id");
                    });
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.DoctorId).HasName("PK_Doctors");

            entity.ToTable("DOCTORS");

            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.FullName)
                .HasMaxLength(150)
                .HasColumnName("full_name");
            entity.Property(e => e.MedicalOrgName)
                .HasMaxLength(200)
                .HasColumnName("medical_org_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Specialty)
                .HasMaxLength(100)
                .HasColumnName("specialty");
        });

        modelBuilder.Entity<Indicator>(entity =>
        {
            entity.HasKey(e => e.IndicatorId).HasName("PK_Indicators");

            entity.ToTable("INDICATORS");

            entity.Property(e => e.IndicatorId).HasColumnName("indicator_id");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.ReferenceRange)
                .HasMaxLength(100)
                .HasColumnName("reference_range");
            entity.Property(e => e.Unit)
                .HasMaxLength(50)
                .HasColumnName("unit");
        });

        modelBuilder.Entity<LabTechnician>(entity =>
        {
            entity.HasKey(e => e.TechnicianId).HasName("PK_LabTechnicians");

            entity.ToTable("LAB_TECHNICIANS");

            entity.Property(e => e.TechnicianId).HasColumnName("technician_id");
            entity.Property(e => e.FullName)
                .HasMaxLength(150)
                .HasColumnName("full_name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK_Orders");

            entity.ToTable("ORDERS");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.OverallStatus)
                .HasMaxLength(50)
                .HasDefaultValue("В обработке")
                .HasColumnName("overall_status");
            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .HasDefaultValue("Ожидает оплаты")
                .HasColumnName("payment_status");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Orders)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Doctors");

            entity.HasOne(d => d.Patient).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Patients");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PK_Patients");

            entity.ToTable("PATIENTS");

            entity.HasIndex(e => e.PassportNumber, "UQ_Patients_Passport").IsUnique();

            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.FullName)
                .HasMaxLength(150)
                .HasColumnName("full_name");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasColumnName("gender");
            entity.Property(e => e.PassportNumber)
                .HasMaxLength(20)
                .HasColumnName("passport_number");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Sample>(entity =>
        {
            entity.HasKey(e => e.SampleId).HasName("PK_Samples");

            entity.ToTable("SAMPLES");

            entity.HasIndex(e => e.Barcode, "UQ_Samples_Barcode").IsUnique();

            entity.Property(e => e.SampleId).HasColumnName("sample_id");
            entity.Property(e => e.AnalysisTypeId).HasColumnName("analysis_type_id");
            entity.Property(e => e.Barcode)
                .HasMaxLength(100)
                .HasColumnName("barcode");
            entity.Property(e => e.CollectedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("collected_at");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Взят")
                .HasColumnName("status");

            entity.HasOne(d => d.AnalysisType).WithMany(p => p.Samples)
                .HasForeignKey(d => d.AnalysisTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Samples_AnalysisTypes");

            entity.HasOne(d => d.Order).WithMany(p => p.Samples)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_Samples_Orders");
        });

        modelBuilder.Entity<TestResult>(entity =>
        {
            entity.HasKey(e => e.ResultId).HasName("PK_TestResults");

            entity.ToTable("TEST_RESULTS");

            entity.Property(e => e.ResultId).HasColumnName("result_id");
            entity.Property(e => e.CompletedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("completed_at");
            entity.Property(e => e.DeviationStatus)
                .HasMaxLength(50)
                .HasDefaultValue("В норме")
                .HasColumnName("deviation_status");
            entity.Property(e => e.IndicatorId).HasColumnName("indicator_id");
            entity.Property(e => e.ResultValue)
                .HasMaxLength(100)
                .HasColumnName("result_value");
            entity.Property(e => e.SampleId).HasColumnName("sample_id");
            entity.Property(e => e.TechnicianId).HasColumnName("technician_id");

            entity.HasOne(d => d.Indicator).WithMany(p => p.TestResults)
                .HasForeignKey(d => d.IndicatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TestResults_Indicators");

            entity.HasOne(d => d.Sample).WithMany(p => p.TestResults)
                .HasForeignKey(d => d.SampleId)
                .HasConstraintName("FK_TestResults_Samples");

            entity.HasOne(d => d.Technician).WithMany(p => p.TestResults)
                .HasForeignKey(d => d.TechnicianId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TestResults_LabTechnicians");
        });

        modelBuilder.Entity<ViewOrder>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_Orders");

            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.Doctor).HasMaxLength(150);
            entity.Property(e => e.DoctorSpecialty).HasMaxLength(100);
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.OverallStatus)
                .HasMaxLength(50)
                .HasColumnName("overall_status");
            entity.Property(e => e.Patient).HasMaxLength(150);
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .HasColumnName("payment_status");
        });

        modelBuilder.Entity<ViewSample>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_Samples");

            entity.Property(e => e.AnalysisType).HasMaxLength(150);
            entity.Property(e => e.Barcode)
                .HasMaxLength(100)
                .HasColumnName("barcode");
            entity.Property(e => e.Biomaterial).HasMaxLength(100);
            entity.Property(e => e.CollectedAt).HasColumnName("collected_at");
            entity.Property(e => e.Patient).HasMaxLength(150);
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.SampleId).HasColumnName("sample_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
        });

        modelBuilder.Entity<ViewTestResult>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_TestResults");

            entity.Property(e => e.CompletedAt).HasColumnName("completed_at");
            entity.Property(e => e.DeviationStatus)
                .HasMaxLength(50)
                .HasColumnName("deviation_status");
            entity.Property(e => e.Indicator).HasMaxLength(150);
            entity.Property(e => e.ReferenceRange)
                .HasMaxLength(100)
                .HasColumnName("reference_range");
            entity.Property(e => e.ResultId).HasColumnName("result_id");
            entity.Property(e => e.ResultValue)
                .HasMaxLength(100)
                .HasColumnName("result_value");
            entity.Property(e => e.SampleBarcode).HasMaxLength(100);
            entity.Property(e => e.Technician).HasMaxLength(150);
            entity.Property(e => e.Unit)
                .HasMaxLength(50)
                .HasColumnName("unit");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
