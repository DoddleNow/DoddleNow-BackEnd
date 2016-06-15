using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DoddleNow.API.Controllers
{
    [RoutePrefix("api/Orders")]
    public class OrdersController : ApiController
    {
        [Authorize]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(Order.CreateOrders());
        }

    }

    #region Helpers

    public class Order
    {
        public int OrderID { get; set; }
        public string CustomerName { get; set; }
        public string ShipperCity { get; set; }
        public Boolean IsShipped { get; set; }

        public static List<Order> CreateOrders()
        {
            List<Order> OrderList = new List<Order>
            {
                new Order {OrderID = 10248, CustomerName = "John Morris", ShipperCity = "San Diego", IsShipped = true },
                new Order {OrderID = 10249, CustomerName = "Bojan Cincur", ShipperCity = "Budva", IsShipped = false},
                new Order {OrderID = 10250,CustomerName = "Aron Grummet", ShipperCity = "New York City", IsShipped = false },
                new Order {OrderID = 10251,CustomerName = "Boris Maksimoic", ShipperCity = "Herceg Novi", IsShipped = false},
                new Order {OrderID = 10252,CustomerName = "Craig Giangregorio", ShipperCity = "Mexico City", IsShipped = true}
            };

            return OrderList;
        }
    }
    #endregion
}
