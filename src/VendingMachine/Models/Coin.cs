using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VendingMachine.Models
{
    public class Coins
    {
        public static int[] Values = new int[] { 10, 5, 2, 1 };
        public Coins()
        {
            Count = 0;
            Value = 0;
        }
        public Coins(int value, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "Значение должно быть больше либо равно нуля.");
            if (!Values.Contains(value))
                throw new ArgumentOutOfRangeException("value", "Монета сомнительного свойства!");

            Count = count;
            Value = value;
        }
        public int Sum { get { return Count >= 0 && Value >= 0 ? Count * Value : 0; } }
        public int Count { get; set; }
        public int Value { get; set; }

        internal void Add(int count = 1)
        {
            Count += count;
        }

        internal void Minus(int count = 1)
        {
            Count -= count;
        }

        public bool Move(int count, ref Coins to)
        {
            if (to.Value != Value)
                throw new InvalidOperationException("Недопустимо перекладывать монеты в кучах неравнозначных монет");
            if (count <= Count)
            {
                this.Minus(count);
                to.Add(count);
                return true;
            }
            else return false;
        }
    }
}
