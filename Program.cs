using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace ProductStoreDatabase
{
    [Serializable]
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public DateTime ExpirationDate { get; set; }

        public Product()
        {
        }

        public Product(
            int id,
            string name,
            string category,
            decimal price,
            int quantity,
            DateTime expirationDate)
        {
            Id = id;
            Name = name;
            Category = category;
            Price = price;
            Quantity = quantity;
            ExpirationDate = expirationDate;
        }

        public override string ToString()
        {
            return $"ID: {Id,-5} " +
                   $"Название: {Name,-15} " +
                   $"Категория: {Category,-12} " +
                   $"Цена: {Price,-8} " +
                   $"Кол-во: {Quantity,-5} " +
                   $"Годен до: {ExpirationDate:d}";
        }
    }

    public static class ProductDatabase
    {
        public static void Save(
    string fileName,
    List<Product> products)
        {
            try
            {
                using BinaryWriter writer =
                    new BinaryWriter(
                        File.Open(fileName, FileMode.Create));

                writer.Write(products.Count);

                foreach (Product product in products)
                {
                    writer.Write(product.Id);
                    writer.Write(product.Name);
                    writer.Write(product.Category);
                    writer.Write(product.Price);
                    writer.Write(product.Quantity);
                    writer.Write(product.ExpirationDate.Ticks);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(
                    $"Ошибка сохранения: {exception.Message}");
            }
        }

        public static List<Product> Load(string fileName)
        {
            List<Product> products = new();

            try
            {
                if (!File.Exists(fileName))
                {
                    return products;
                }

                using BinaryReader reader =
                    new BinaryReader(
                        File.Open(fileName, FileMode.Open));

                int count = reader.ReadInt32();

                for (int i = 0; i < count; i++)
                {
                    products.Add(
                        new Product(
                            reader.ReadInt32(),
                            reader.ReadString(),
                            reader.ReadString(),
                            reader.ReadDecimal(),
                            reader.ReadInt32(),
                            new DateTime(reader.ReadInt64())));
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(
                    $"Ошибка загрузки: {exception.Message}");
            }

            return products;
        }

        public static void AddProduct(
            List<Product> products,
            Product product)
        {
            products.Add(product);
        }

        public static bool DeleteProduct(
            List<Product> products,
            int id)
        {
            Product product =
                products.FirstOrDefault(item => item.Id == id);

            if (product == null)
            {
                return false;
            }

            products.Remove(product);

            return true;
        }

        public static List<Product> GetByCategory(
            List<Product> products,
            string category)
        {
            return products
                .Where(product =>
                    product.Category.Equals(
                        category,
                        StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public static List<Product> GetCheaperThan(
            List<Product> products,
            decimal maxPrice)
        {
            return products
                .Where(product => product.Price < maxPrice)
                .ToList();
        }

        public static decimal GetTotalCost(
            List<Product> products)
        {
            return products.Sum(
                product => product.Price * product.Quantity);
        }

        public static decimal GetAveragePrice(
            List<Product> products)
        {
            return products.Count == 0
                ? 0
                : products.Average(product => product.Price);
        }

        public static void ShowAll(
            List<Product> products)
        {
            if (!products.Any())
            {
                Console.WriteLine("База данных пуста.");
                return;
            }

            foreach (Product product in products)
            {
                Console.WriteLine(product);
            }
        }
    }

    internal static class InputHelper
    {
        public static int ReadInt(string message)
        {
            while (true)
            {
                Console.Write(message);

                if (int.TryParse(Console.ReadLine(), out int value))
                {
                    return value;
                }

                Console.WriteLine("Некорректное число.");
            }
        }

        public static decimal ReadDecimal(string message)
        {
            while (true)
            {
                Console.Write(message);

                if (decimal.TryParse(
                        Console.ReadLine(),
                        out decimal value)
                    && value >= 0)
                {
                    return value;
                }

                Console.WriteLine(
                    "Введите неотрицательное число.");
            }
        }

        public static string ReadString(string message)
        {
            while (true)
            {
                Console.Write(message);

                string value = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value;
                }

                Console.WriteLine("Строка не может быть пустой.");
            }
        }

        public static DateTime ReadDate(string message)
        {
            while (true)
            {
                Console.Write(message);

                if (DateTime.TryParse(
                        Console.ReadLine(),
                        out DateTime value))
                {
                    return value;
                }

                Console.WriteLine("Некорректная дата.");
            }
        }
    }

    internal class Program
    {
        private const string FileName = "products.dat";

        private static void Main()
        {
            List<Product> products =
                ProductDatabase.Load(FileName);

            bool exit = false;

            while (!exit)
            {
                Console.WriteLine();
                Console.WriteLine("===== МАГАЗИН ПРОДУКТОВ =====");
                Console.WriteLine("1. Просмотр БД");
                Console.WriteLine("2. Добавить товар");
                Console.WriteLine("3. Удалить товар");
                Console.WriteLine("4. Товары по категории");
                Console.WriteLine("5. Товары дешевле цены");
                Console.WriteLine("6. Общая стоимость склада");
                Console.WriteLine("7. Средняя цена товара");
                Console.WriteLine("8. Сохранить БД");
                Console.WriteLine("0. Выход");

                int choice =
                    InputHelper.ReadInt("Выберите пункт: ");

                Console.WriteLine();

                switch (choice)
                {
                    case 1:
                        ProductDatabase.ShowAll(products);
                        break;

                    case 2:
                        AddProduct(products);
                        break;

                    case 3:
                        DeleteProduct(products);
                        break;

                    case 4:
                        FindByCategory(products);
                        break;

                    case 5:
                        FindCheaper(products);
                        break;

                    case 6:
                        Console.WriteLine(
                            $"Общая стоимость: " +
                            $"{ProductDatabase.GetTotalCost(products)}");
                        break;

                    case 7:
                        Console.WriteLine(
                            $"Средняя цена: " +
                            $"{ProductDatabase.GetAveragePrice(products):F2}");
                        break;

                    case 8:
                        ProductDatabase.Save(
                            FileName,
                            products);

                        Console.WriteLine("База сохранена.");
                        break;

                    case 0:
                        ProductDatabase.Save(
                            FileName,
                            products);

                        exit = true;
                        break;

                    default:
                        Console.WriteLine(
                            "Такого пункта меню нет.");
                        break;
                }
            }
        }

        private static void AddProduct(
            List<Product> products)
        {
            int id =
                InputHelper.ReadInt("ID: ");

            if (products.Any(product => product.Id == id))
            {
                Console.WriteLine(
                    "Товар с таким ID уже существует.");
                return;
            }

            string name =
                InputHelper.ReadString("Название: ");

            string category =
                InputHelper.ReadString("Категория: ");

            decimal price =
                InputHelper.ReadDecimal("Цена: ");

            int quantity =
                InputHelper.ReadInt("Количество: ");

            DateTime expirationDate =
                InputHelper.ReadDate("Срок годности: ");

            Product product =
                new Product(
                    id,
                    name,
                    category,
                    price,
                    quantity,
                    expirationDate);

            ProductDatabase.AddProduct(
                products,
                product);

            Console.WriteLine("Товар добавлен.");
        }

        private static void DeleteProduct(
            List<Product> products)
        {
            int id =
                InputHelper.ReadInt(
                    "Введите ID для удаления: ");

            if (ProductDatabase.DeleteProduct(
                    products,
                    id))
            {
                Console.WriteLine(
                    "Товар удален.");
            }
            else
            {
                Console.WriteLine(
                    "Товар не найден.");
            }
        }

        private static void FindByCategory(
            List<Product> products)
        {
            string category =
                InputHelper.ReadString(
                    "Введите категорию: ");

            List<Product> result =
                ProductDatabase.GetByCategory(
                    products,
                    category);

            if (!result.Any())
            {
                Console.WriteLine(
                    "Ничего не найдено.");
                return;
            }

            foreach (Product product in result)
            {
                Console.WriteLine(product);
            }
        }

        private static void FindCheaper(
            List<Product> products)
        {
            decimal price =
                InputHelper.ReadDecimal(
                    "Максимальная цена: ");

            List<Product> result =
                ProductDatabase.GetCheaperThan(
                    products,
                    price);

            if (!result.Any())
            {
                Console.WriteLine(
                    "Ничего не найдено.");
                return;
            }

            foreach (Product product in result)
            {
                Console.WriteLine(product);
            }
        }
    }
}