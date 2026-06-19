using System;
using System.Collections.Generic;
using System.Linq;
using ProductStoreDatabase.Models;
using ProductStoreDatabase.Services;

namespace Lab_8
{
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
