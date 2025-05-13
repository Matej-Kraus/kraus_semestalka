using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using kraus_semestalka.Data.Models;

namespace kraus_semestalka.Data;

public partial class VAPW_PS_DrivesContext : DbContext
{
    public VAPW_PS_DrivesContext()
    {
    }

    public VAPW_PS_DrivesContext(DbContextOptions<VAPW_PS_DrivesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DriveData> DriveData { get; set; }

    public virtual DbSet<Recordings> Recordings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=147.230.21.212;Initial Catalog=VAPW_PS_Drives;User ID=vapw;Password=cv1k0;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DriveData>(entity =>
        {
            entity.Property(e => e.AxCalibrationStatusInfo).HasMaxLength(32);
            entity.Property(e => e.AyCalibrationStatusInfo).HasMaxLength(32);
            entity.Property(e => e.AzCalibrationStatusInfo).HasMaxLength(32);
            entity.Property(e => e.GxCalibrationStatusInfo).HasMaxLength(32);
            entity.Property(e => e.GyCalibrationStatusInfo).HasMaxLength(32);
            entity.Property(e => e.GzCalibrationStatusInfo).HasMaxLength(32);
            entity.Property(e => e.ImuStatusInfo).HasMaxLength(32);

            entity.HasOne(d => d.Recording).WithMany(p => p.DriveData).HasForeignKey(d => d.RecordingId);
        });

        modelBuilder.Entity<Recordings>(entity =>
        {
            entity.Property(e => e.SensorsDeviceName).HasDefaultValue("");
            entity.Property(e => e.UIID)
                .HasMaxLength(32)
                .HasDefaultValue("");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent).HasForeignKey(d => d.ParentId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
