using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VendingMachine.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        internal Product Buy(int count = 1)
        {
            if (Count < count)
                throw new InvalidCastException("Указанного количества товара не существует");
            return new Product { Id = Id, Name = Name, Price = Price, Count = count };
        }
    }
}
