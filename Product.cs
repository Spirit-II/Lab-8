using System;

namespace ProductStoreDatabase.Models
{
    /// <summary>
    /// Товар магазина.
    /// </summary>
    [Serializable]
    public class Product
    {
        private int _id;
        private string _name;
        private string _category;
        private decimal _price;
        private int _quantity;
        private DateTime _expirationDate;

        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Category
        {
            get => _category;
            set => _category = value;
        }

        public decimal Price
        {
            get => _price;
            set => _price = value;
        }

        public int Quantity
        {
            get => _quantity;
            set => _quantity = value;
        }

        public DateTime ExpirationDate
        {
            get => _expirationDate;
            set => _expirationDate = value;
        }

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
}