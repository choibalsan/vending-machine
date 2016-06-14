using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using VendingMachine.Models;
using System.Net.Http;
using System.Web.Http;
using System.Net;

namespace VendingMachine.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        /// <summary>
        /// товар
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return Vending.Instance.StoreGoods;
        }

        /// <summary>
        /// купить товар
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("Buy/{id}", Name = "Buy")]
        public Product Buy(int id)
        {
            try
            {
                return Vending.Instance.BuyGoods(id);
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
