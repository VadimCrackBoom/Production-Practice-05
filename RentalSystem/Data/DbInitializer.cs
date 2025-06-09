// Data/DbInitializer.cs
using RentalSystem.Models;

namespace RentalSystem.Data
{
    public static class DbInitializer
    {
        public static void Initialize(RentalDbContext context)
        {
            if (context.Equipment.Any())
            {
                return; // База данных уже инициализирована
            }

            var equipment = new Equipment[]
            {
                new Equipment { Name = "Горные лыжи Atomic", Type = "Лыжи", Size = "160cm", Condition = "Отличное", RentalPricePerHour = 500, IsAvailable = true },
                new Equipment { Name = "Горные лыжи Head", Type = "Лыжи", Size = "170cm", Condition = "Хорошее", RentalPricePerHour = 450, IsAvailable = true },
                new Equipment { Name = "Сноуборд Burton", Type = "Сноуборд", Size = "155cm", Condition = "Отличное", RentalPricePerHour = 600, IsAvailable = true },
                new Equipment { Name = "Ботинки лыжные", Type = "Ботинки", Size = "42", Condition = "Хорошее", RentalPricePerHour = 200, IsAvailable = true },
                new Equipment { Name = "Ботинки сноубордические", Type = "Ботинки", Size = "43", Condition = "Новое", RentalPricePerHour = 250, IsAvailable = true },
                new Equipment { Name = "Палки лыжные", Type = "Палки", Size = "120cm", Condition = "Хорошее", RentalPricePerHour = 100, IsAvailable = true },
                new Equipment { Name = "Шлем защитный", Type = "Шлем", Size = "M", Condition = "Отличное", RentalPricePerHour = 150, IsAvailable = true }
            };

            context.Equipment.AddRange(equipment);
            context.SaveChanges();
        }
    }
}