using System;

namespace WtLab2._2oop.Models
{
    public class Truck : Car
    {
        public double LoadCapacityTons { get; set; }

        public Truck(int id, string brand, string model, int year, decimal pricePerDay,
                     bool isAvailable, double loadCapacityTons)
            : base(id, brand, model, year, pricePerDay, isAvailable)
        {
            LoadCapacityTons = loadCapacityTons;
        }

        public override void PrintInfo()
        {
            base.PrintInfo();
            Console.WriteLine($"Грузоподъёмность: {LoadCapacityTons} т");
            Console.WriteLine("----------------------------------------");
        }

        public override string GetShortInfo()
        {
            return $"Грузовик {Brand} {Model} — {LoadCapacityTons} т";
        }
    }
}