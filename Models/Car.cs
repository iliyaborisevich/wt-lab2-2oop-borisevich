using System;

namespace WtLab2._2oop.Models
{
    public class Car : BaseEntity
    {
        private decimal _pricePerDay;

        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public bool IsAvailable { get; set; }

        public decimal PricePerDay
        {
            get { return _pricePerDay; }
            private set { _pricePerDay = value; }
        }

        public Car(int id, string brand, string model, int year, decimal pricePerDay, bool isAvailable)
        {
            Id = id;
            Brand = brand;
            Model = model;
            Year = year;
            IsAvailable = isAvailable;
            UpdatePrice(pricePerDay);
        }

        // Цена не может быть отрицательной
        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice < 0)
                throw new ArgumentException("Цена не может быть отрицательной.");
            _pricePerDay = newPrice;
        }

        public override void PrintInfo()
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"ID:           {Id}");
            Console.WriteLine($"Марка:        {Brand}");
            Console.WriteLine($"Модель:       {Model}");
            Console.WriteLine($"Год выпуска:  {Year}");
            Console.WriteLine($"Цена за день: {PricePerDay:C}");
            Console.WriteLine($"Доступен:     {(IsAvailable ? "да" : "нет")}");
            Console.WriteLine("----------------------------------------");
        }

        public override string GetShortInfo()
        {
            return $"Авто {Brand} {Model} ({Year}) — {PricePerDay:C}/день";
        }
    }
}