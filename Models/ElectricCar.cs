using System;

namespace WtLab2._2oop.Models
{
    public class ElectricCar : Car
    {
        public int BatteryRangeKm { get; set; }

        public ElectricCar(int id, string brand, string model, int year, decimal pricePerDay,
                           bool isAvailable, int batteryRangeKm)
            : base(id, brand, model, year, pricePerDay, isAvailable)
        {
            BatteryRangeKm = batteryRangeKm;
        }

        public override void PrintInfo()
        {
            base.PrintInfo();
            Console.WriteLine($"Запас хода:      {BatteryRangeKm} км");
            Console.WriteLine("----------------------------------------");
        }

        public override string GetShortInfo()
        {
            return $"Электромобиль {Brand} {Model} — запас {BatteryRangeKm} км";
        }
    }
}