using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using VendingMachine.Models;

namespace VendingMachine.Controllers
{
    [Route("api/[controller]")]
    public class StoreController : Controller
    {

        /// <summary>
        /// кошелек VM для сдачи
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public IEnumerable<Coins> Get()
        {
            return Vending.Instance.StoreWallet.Money.Values;
        }
    }
}
