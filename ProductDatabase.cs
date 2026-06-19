using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProductStoreDatabase.Models;

namespace ProductStoreDatabase.Services
{
    /// <summary>
    /// Работа с базой данных товаров.
    /// </summary>
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
}