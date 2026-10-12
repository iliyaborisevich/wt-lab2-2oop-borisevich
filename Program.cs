using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WtLab2._2oop.Models;
using WtLab2._2oop.Services;
using WtLab2._2oop.Interfaces;

namespace WtLab2._2oop
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                Console.WriteLine();
Console.WriteLine("╔══════════════════════════════════════════╗");
Console.WriteLine("║      АВТОСАЛОН «КОЛЕСО»                  ║");
Console.WriteLine("║      Аренда автомобилей                  ║");
Console.WriteLine("╠══════════════════════════════════════════╣");
Console.WriteLine("║  ОСНОВНЫЕ ОПЕРАЦИИ                       ║");
Console.WriteLine("║   1. Список транспортных средств         ║");
Console.WriteLine("║   2. Краткая информация о ТС             ║");
Console.WriteLine("║   3. Изменить цену автомобиля            ║");
Console.WriteLine("║   4. Работа с сервисом (поиск/удаление)  ║");
Console.WriteLine("╠══════════════════════════════════════════╣");
Console.WriteLine("║  КОЛЛЕКЦИИ И LINQ                        ║");
Console.WriteLine("║   5. Демонстрация List<Car>              ║");
Console.WriteLine("║   6. Демонстрация Dictionary             ║");
Console.WriteLine("║   7. LINQ: фильтр, агрегаты, группировка ║");
Console.WriteLine("╠══════════════════════════════════════════╣");
Console.WriteLine("║  АСИНХРОННОСТЬ                           ║");
Console.WriteLine("║   8. Асинхронная загрузка                ║");
Console.WriteLine("║   9. Асинхронное получение коллекции     ║");
Console.WriteLine("║  10. Параллельная загрузка + отмена      ║");
Console.WriteLine("╠══════════════════════════════════════════╣");
Console.WriteLine("║   0. Выход                               ║");
Console.WriteLine("╚══════════════════════════════════════════╝");
Console.Write("  Ваш выбор: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1": ShowAllVehicles(); break;
                    case "2": ShowShortInfo(); break;
                    case "3": ChangePrice(); break;
                    case "4": ServiceDemo(); break;
                    case "5": ShowListDemo(); break;
                    case "6": ShowDictionaryDemo(); break;
                    case "7": ShowLinqDemo(); break;

                    case "8":
                        var loader = new DataLoader();
                        Console.WriteLine(">>> Асинхронная загрузка:");
                        await loader.LoadAsync();
                        break;

                    case "9":
                        var loader2 = new DataLoader();
                        Console.WriteLine(">>> Асинхронное получение коллекции:");
                        var loaded = await loader2.GetCarsAsync();
                        foreach (var c in loaded)
                            Console.WriteLine("  " + c.GetShortInfo());
                        break;

                    case "10":
                        var loader3 = new DataLoader();

                        Console.WriteLine(">>> Параллельная загрузка (Task.WhenAll):");
                        var t1 = loader3.LoadPartAsync("Часть A", 1000, CancellationToken.None);
                        var t2 = loader3.LoadPartAsync("Часть B", 1500, CancellationToken.None);
                        var t3 = loader3.LoadPartAsync("Часть C", 800, CancellationToken.None);
                        var results = await Task.WhenAll(t1, t2, t3);
                        foreach (var r in results)
                            Console.WriteLine("  " + r);

                        Console.WriteLine();
                        Console.WriteLine(">>> Отмена через CancellationToken:");
                        var cts = new CancellationTokenSource();
                        cts.CancelAfter(500);
                        try
                        {
                            await loader3.LoadPartAsync("Долгая операция", 3000, cts.Token);
                            Console.WriteLine("  Успешно завершено.");
                        }
                        catch (TaskCanceledException)
                        {
                            Console.WriteLine("  Операция отменена по таймауту.");
                        }
                        break;

                    case "0": Console.WriteLine("Выход."); return;
                    default: Console.WriteLine("Неверный пункт."); break;
                }
            }
        }

        // ===== Блок 1 =====

        static BaseEntity[] GetFleet()
        {
            return new BaseEntity[]
            {
                new Car(1, "Toyota", "Camry", 2022, 75.00m, true),
                new Car(2, "BMW", "X5", 2021, 150.00m, true),
                new Truck(3, "Volvo", "FH16", 2020, 200.00m, true, 20.0),
                new ElectricCar(4, "Tesla", "Model 3", 2023, 120.00m, true, 500)
            };
        }

        static void ShowAllVehicles()
        {
            foreach (BaseEntity item in GetFleet())
                item.PrintInfo();
        }

        static void ShowShortInfo()
        {
            foreach (BaseEntity item in GetFleet())
                Console.WriteLine("  " + item.GetShortInfo());
        }

        static void ChangePrice()
        {
            Car car = (Car)GetFleet()[0];
            Console.WriteLine($"Текущая цена {car.Brand} {car.Model}: {car.PricePerDay:C}");
            Console.Write("Введите новую цену: ");

            if (!decimal.TryParse(Console.ReadLine(), out decimal newPrice))
            {
                Console.WriteLine("Неверный формат.");
                return;
            }

            try
            {
                car.UpdatePrice(newPrice);
                Console.WriteLine($"Новая цена: {car.PricePerDay:C}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }

        // ===== Блок 1, Уровень 3 =====

        static void ServiceDemo()
        {
            IItemService<Car> service = new CarService();
            service.Add(new Car(1, "Toyota", "Camry", 2022, 75m, true));
            service.Add(new Car(2, "BMW", "X5", 2021, 150m, true));
            service.Add(new Car(3, "Kia", "Rio", 2020, 45m, false));

            Console.WriteLine(">>> Все через интерфейсную ссылку:");
            foreach (var c in service.GetAll())
                Console.WriteLine("  " + c.GetShortInfo());

            Console.WriteLine();
            Console.Write("Поиск по марке: ");
            string brand = Console.ReadLine();
            var found = ((CarService)service).Find(brand);
            if (found.Count == 0) Console.WriteLine("  Не найдено.");
            else foreach (var c in found) Console.WriteLine("  " + c.GetShortInfo());

            Console.WriteLine();
            Console.Write("Удалить по Id: ");
            if (int.TryParse(Console.ReadLine(), out int id))
                Console.WriteLine(service.Remove(id) ? "  Удалено." : "  Не найдено.");
            else
                Console.WriteLine("  Неверный Id.");

            Console.WriteLine($"  Осталось записей: {service.Count}");
        }

        // ===== Блок 2, Уровень 1 =====

        static void ShowListDemo()
        {
            List<Car> cars = new List<Car>
            {
                new Car(1, "Toyota", "Camry", 2022, 75m, true),
                new Car(2, "BMW", "X5", 2021, 150m, true),
                new Car(3, "Kia", "Rio", 2020, 45m, false),
                new Car(4, "Volvo", "FH16", 2019, 200m, true),
                new Car(5, "Tesla", "Model 3", 2023, 120m, true),
                new Car(6, "Audi", "A6", 2022, 130m, true)
            };

            Console.WriteLine(">>> List<Car> через foreach:");
            foreach (var car in cars)
                Console.WriteLine("  " + car.GetShortInfo());
        }

        // ===== Блок 2, Уровень 2 =====

        static void ShowDictionaryDemo()
        {
            Dictionary<int, Car> carsById = new Dictionary<int, Car>
            {
                { 1, new Car(1, "Toyota", "Camry", 2022, 75m, true) },
                { 2, new Car(2, "BMW", "X5", 2021, 150m, true) },
                { 3, new Car(3, "Kia", "Rio", 2020, 45m, false) }
            };

            Console.WriteLine(">>> Dictionary<Id, Car> создан:");
            foreach (var pair in carsById)
                Console.WriteLine($"  Key={pair.Key}: {pair.Value.GetShortInfo()}");

            Console.WriteLine();
            Console.Write("Введите Id для поиска: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неверный формат.");
                return;
            }

            if (carsById.TryGetValue(id, out Car found))
                Console.WriteLine("  Найдено: " + found.GetShortInfo());
            else
                Console.WriteLine("  Автомобиль с таким Id не найден.");
        }

        // ===== Блок 2, Уровень 3 =====

        static void ShowLinqDemo()
        {
            List<Car> cars = new List<Car>
            {
                new Car(1, "Toyota", "Camry", 2022, 75m, true),
                new Car(2, "BMW", "X5", 2021, 150m, true),
                new Car(3, "Kia", "Rio", 2020, 45m, false),
                new Car(4, "Volvo", "FH16", 2019, 200m, true),
                new Car(5, "Tesla", "Model 3", 2023, 120m, true),
                new Car(6, "Audi", "A6", 2022, 130m, true)
            };

            var filtered = cars
                .Where(c => c.IsAvailable && c.PricePerDay < 150)
                .OrderByDescending(c => c.PricePerDay)
                .Select(c => $"{c.Brand} {c.Model} — {c.PricePerDay:C}")
                .ToList();

            Console.WriteLine(">>> Where + OrderBy + Select:");
            filtered.ForEach(s => Console.WriteLine("  " + s));

            Console.WriteLine();
            Console.WriteLine($">>> Count: {cars.Count}");
            Console.WriteLine($">>> Sum:   {cars.Sum(c => c.PricePerDay):C}");
            Console.WriteLine($">>> Avg:   {cars.Average(c => c.PricePerDay):C}");
            Console.WriteLine($">>> Min:   {cars.Min(c => c.PricePerDay):C}");
            Console.WriteLine($">>> Max:   {cars.Max(c => c.PricePerDay):C}");

            Console.WriteLine();
            Console.WriteLine(">>> GroupBy по году:");
            foreach (var group in cars.GroupBy(c => c.Year))
                Console.WriteLine($"  {group.Key}: {group.Count()} шт.");
        }
    }
}