using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using VendingMachine.Models.Extensions;

namespace VendingMachine.Models
{
    public sealed class Vending
    {
        private static readonly Vending instance = new Vending();

        static Vending()
        {
        }
        public static Vending Instance
        {
            get
            {
                return instance;
            }
        }
        public Wallet ClientWallet { get; private set; }
        public Wallet StoreWallet { get; private set; }
        public List<Product> StoreGoods { get; private set; }
        private Vending()
        {
            // TODO test data
            ClientWallet = new Wallet(new Dictionary<int, Coins>(){
                { 1,new Coins(1,10) },
                { 2,new Coins(2,30) },
                { 5,new Coins(5,20) },
                { 10,new Coins(10,15) },
            });

            StoreWallet = new Wallet(new Dictionary<int, Coins>(){
                { 1,new Coins(1,100) },
                { 2,new Coins(2,100) },
                { 5,new Coins(5,100) },
                { 10,new Coins(10,100) },
            });

            StoreGoods = new List<Product>() {
                new Product {Id=1,Name="Чай",Count=10,Price=13 },
                new Product {Id=2,Name="Кофе",Count=20,Price=18 },
                new Product {Id=3,Name="Кофе с молоком",Count=20,Price=21 },
                new Product {Id=4,Name="Сок",Count=15,Price=35 },
            };

        }

        internal void AddClientMoney(int value)
        {
            ClientWallet.AddCoin(value);
        }

        internal IList<Coins> ReturnClientMoney()
        {
            // временно объединяем кошельки клиента и машины
            Wallet tempWallet = GetUnitedWallet();
            // получили монетки для сдачи
            Dictionary<int, Coins> result = tempWallet.GetSummInCoins(ClientWallet.Money.CoinsSum());
            // по факту получения корректной кучи монет очищаем кошелек клиента и переносим все остальное в кошелек машины
            StoreWallet = tempWallet;
            ClientWallet.Clear();
            return result.Select(p => p.Value).ToList();
        }
        internal Product BuyGoods(int id)
        {
            Product possibleBuy = StoreGoods.FirstOrDefault(p => p.Id == id);
            if (possibleBuy == null)
                throw new InvalidOperationException("Товара с указанным ид не существует");
            if (possibleBuy.Count < 1)
                throw new InvalidOperationException("Товара с указанным ид нет в наличии");
            if (ClientWallet.Money.CoinsSum() <= possibleBuy.Price)
                throw new InvalidOperationException("Недостаточно средств");

            // временно объединяем кошельки клиента и машины
            Wallet tempWallet = GetUnitedWallet();
            // получение монет для перевода в кошелек пользователя после покупки
            Dictionary<int, Coins> result = tempWallet.GetSummInCoins(ClientWallet.Money.CoinsSum() - possibleBuy.Price);

            StoreWallet = tempWallet;
            ClientWallet = new Wallet(result);
            // "продаем" товар
            return possibleBuy.Buy();
        }

        private Wallet GetUnitedWallet()
        {
            Wallet tempWallet = new Wallet(StoreWallet.Money);
            foreach (var pair in ClientWallet.Money)
                if (tempWallet.Money.ContainsKey(pair.Key))
                    tempWallet.Money[pair.Key].Add(pair.Value.Count);
                else
                    tempWallet.Money.Add(pair.Key, pair.Value);
            return tempWallet;
        }
    }
}
