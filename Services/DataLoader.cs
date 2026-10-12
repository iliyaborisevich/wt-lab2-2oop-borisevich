using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WtLab2._2oop.Models;

namespace WtLab2._2oop.Services
{
    public class DataLoader
    {
    
        public async Task LoadAsync()
        {
            Console.WriteLine("  Загрузка началась...");
            await Task.Delay(1000);
            Console.WriteLine("  Загрузка завершена.");
        }

        public async Task<List<Car>> GetCarsAsync()
        {
            try
            {
                Console.WriteLine("  Запрос к базе...");
                await Task.Delay(1000);

                var cars = new List<Car>
                {
                    new Car(1, "Toyota", "Camry", 2022, 75m, true),
                    new Car(2, "BMW", "X5", 2021, 150m, true)
                };

                Console.WriteLine("  Данные получены.");
                return cars;
            }
            catch (Exception ex)
            {
                Console.WriteLine("  Ошибка: " + ex.Message);
                return new List<Car>();
            }
            finally
            {
                Console.WriteLine("  Запрос завершён (finally).");
            }
        }

        // Уровень 3: часть для параллельной загрузки
        public async Task<string> LoadPartAsync(string name, int delayMs, CancellationToken token)
        {
            await Task.Delay(delayMs, token);
            return $"{name} готов (за {delayMs} мс)";
        }
    }
}