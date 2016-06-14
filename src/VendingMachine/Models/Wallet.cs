
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendingMachine.Models.Extensions;

namespace VendingMachine.Models
{
    public class Wallet
    {
        public Dictionary<int, Coins> Money { get; private set; }
        public Wallet()
        {
            Money = new Dictionary<int, Coins>();
        }
        public Wallet(Dictionary<int, Coins> initialMoney)
        {
            if (initialMoney == null)
                throw new ArgumentNullException("initialMoney");
            Money = initialMoney;
        }
        internal void AddCoin(int value)
        {
            if (Money.ContainsKey(value))
                Money[value].Add();
            else Money.Add(value, new Coins(value, 1));
        }
        internal void Clear()
        {
            if (Money != null)
                Money.Clear();
        }
        /// <summary>
        /// получение заданной суммы денег из кошелька в монетах
        /// </summary>
        /// <param name="summ"></param>
        /// <returns></returns>
        internal Dictionary<int, Coins> GetSummInCoins(int summ)
        {
            Dictionary<int, Coins> result = new Dictionary<int, Coins>();
            int moneySumm = Money.CoinsSum();
            if (moneySumm < summ)
                throw new InvalidOperationException("Запрошенная сумма превышает содержимое кошелька");
            else if (moneySumm == summ)
            {
                result = new Dictionary<int, Coins>(Money);
                Money.Clear();
                return result;
            }
            // сортируем для последующего использования в наборе суммы
            Money = Money.OrderByDescending(v => v.Key).ToDictionary(v => v.Key, v => v.Value);
            // формируем пул доступных монет
            Dictionary<int, int> availableCoins = Money.Where(c => c.Value.Count > 0).ToDictionary(v => v.Key, v => 0);
            foreach (var coins in Money)
                if (summ > 0 && availableCoins.ContainsKey(coins.Key))
                {
                    // количество монет для получения максимально возможной суммы меньше запрошенной
                    availableCoins[coins.Key] = Math.Min(summ / coins.Key, coins.Value.Count);
                    summ -= availableCoins[coins.Key] * coins.Key;
                }
            if (summ == 0)
            {
                // переместить монеты из кошелька в выдачу
                foreach (var coins in availableCoins)
                    if (coins.Value != 0)
                    {
                        Coins tempCoins = new Coins(coins.Key, 0);
                        if (Money[coins.Key].Move(coins.Value, ref tempCoins))
                            result.Add(coins.Key, tempCoins);
                    }
                return result;
            }
            else
                throw new InvalidOperationException("Запрошенная сумма не может быть выдана");
        }

        /// <summary>
        /// сохраняет состояние кошелька
        /// </summary>
        private void SaveState()
        {
            // todo save to file or db
            throw new NotImplementedException();
        }
        private void RestoreState()
        {
            // todo get state from file or db
            throw new NotImplementedException();
        }

    }
}
