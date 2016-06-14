using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using System.Net;
using VendingMachine.Models;
using System.Web.Http;
using System.Net.Http;

namespace VendingMachine.Controllers
{
    [Route("api/[controller]")]
    public class WalletController : Controller
    {
        /// <summary>
        /// кошелек клиента
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IList<Coins> Get()
        {
            return Vending.Instance.ClientWallet.Money.Values.ToList();
        }
        /// <summary>
        /// сдача
        /// </summary>
        /// <returns></returns>
        [HttpPost("ReturnRest", Name = "ReturnRest")]
        public IList<Coins> ReturnRest()
        {
            try
            {
                return Vending.Instance.ReturnClientMoney();
            }
            catch (InvalidOperationException ex)
            {
                var response = new HttpResponseMessage(HttpStatusCode.Conflict);
                response.Content = new StringContent(ex.Message);
                throw new HttpResponseException(response);
            }
        }

        /// <summary>
        /// добавить монету
        /// </summary>
        /// <param name="value"></param>
        [HttpPut("AddCoin/{value}", Name = "AddCoin")]
        public IList<Coins> AddCoin(int value)
        {
            try
            {
                Vending.Instance.AddClientMoney(value);
                return Vending.Instance.ClientWallet.Money.Values.ToList();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                var response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                response.Content = new StringContent(ex.Message);
                throw new HttpResponseException(response);
            }
            catch (InvalidOperationException ex)
            {
                var response = new HttpResponseMessage(HttpStatusCode.Conflict);
                response.Content = new StringContent(ex.Message);
                throw new HttpResponseException(response);
            }
        }

    }
}
