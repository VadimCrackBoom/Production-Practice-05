using RentalSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace RentalSystem.Data;

 public class RentalDbContext : DbContext
    {
        public RentalDbContext(DbContextOptions<RentalDbContext> options) : base(options)
        {
        }

        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<RentalEquipment> RentalEquipment { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка связи многие-ко-многим между Rental и Equipment через RentalEquipment
            modelBuilder.Entity<RentalEquipment>()
                .HasKey(re => new { re.RentalId, re.EquipmentId });

            modelBuilder.Entity<RentalEquipment>()
                .HasOne(re => re.Rental)
                .WithMany(r => r.RentalEquipments)
                .HasForeignKey(re => re.RentalId);

            modelBuilder.Entity<RentalEquipment>()
                .HasOne(re => re.Equipment)
                .WithMany()
                .HasForeignKey(re => re.EquipmentId);

            // Настройка индексов для улучшения производительности
            modelBuilder.Entity<Equipment>()
                .HasIndex(e => e.IsAvailable);

            modelBuilder.Entity<Rental>()
                .HasIndex(r => r.Status);

            modelBuilder.Entity<Rental>()
                .HasIndex(r => r.RentalStart);

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.LastName);

            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Username)
                .IsUnique();

            // Инициализация начальных данных
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    Username = "admin",
                    Password = "admin123",
                    Role = "admin",
                    FirstName = "Admin",
                    LastName = "Adminov",
                    PhoneNumber = "+1234567890",
                    HireDate = DateTime.UtcNow
                }
            );
        }
    }