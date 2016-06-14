using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VendingMachine.Models.Extensions
{
    public static class WalletExtensions
    {
        public static int CoinsSum(this Dictionary<int, Coins> coins)
        {
            return coins.Sum(c => c.Value.Sum);
        }
    }
}
