using System;
using System.Collections.Generic;
using System.Linq;
using WtLab2._2oop.Interfaces;
using WtLab2._2oop.Models;

namespace WtLab2._2oop.Services
{
    public class CarService : IItemService<Car>
    {
        private readonly List<Car> _cars = new List<Car>();

        public int Count => _cars.Count;

        public void Add(Car item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (_cars.Any(c => c.Id == item.Id))
                throw new InvalidOperationException($"Автомобиль с Id={item.Id} уже есть.");

            _cars.Add(item);
        }

        public bool Remove(int id)
        {
            var car = _cars.FirstOrDefault(c => c.Id == id);
            if (car == null) return false;
            _cars.Remove(car);
            return true;
        }

        public Car GetById(int id)
        {
            return _cars.FirstOrDefault(c => c.Id == id);
        }

        public List<Car> GetAll()
        {
            return new List<Car>(_cars);
        }

        // Перегрузка: поиск по марке
        public List<Car> Find(string brand)
        {
            return _cars.Where(c => c.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // Перегрузка: поиск по году
        public List<Car> Find(int year)
        {
            return _cars.Where(c => c.Year == year).ToList();
        }
    }
}