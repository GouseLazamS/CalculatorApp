using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Calculator.DAL.Models;

public partial class CalculatorDbContext : DbContext
{
    public CalculatorDbContext()
    {
    }

    public CalculatorDbContext(DbContextOptions<CalculatorDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CalculationHistory> CalculationHistories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CalculatorDB;Integrated Security=True;Trust Server Certificate=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CalculationHistory>(entity =>
        {
            entity.HasKey(e => e.CalculationId).HasName("PK__Calculat__57C05F66E5503901");

            entity.ToTable("CalculationHistory");

            entity.HasIndex(e => e.CreatedAt, "IX_CreatedAt").IsDescending();

            entity.Property(e => e.CalculationId).HasColumnName("CalculationID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.Expression).IsRequired();
            entity.Property(e => e.OperationType).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
