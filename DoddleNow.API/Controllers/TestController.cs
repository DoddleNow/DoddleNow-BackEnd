using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DoddleNow.API.Controllers
{
    ///<summary>
    ///Orders related controller (test only)
    ///</summary>
    [RoutePrefix("api/Test")]
    public class TestController : ApiController
    {
        ///<summary>
        ///Test GET request for DoddleNow Admin Role (1) only.  Returns list of fake orders
        ///</summary>
        [HttpGet]
        [Authorize(Roles="1")]
        [Route("Roles/DoddleNowAdmin_1")]
        public IHttpActionResult DoddleNowAdminTest()
        {
            return Ok(Order.CreateOrders());
        }

        ///<summary>
        ///Test GET request for DoddleNow Support Role (2) only.  Returns list of fake orders
        ///</summary>
        [HttpGet]
        [Authorize(Roles = "2")]
        [Route("Roles/DoddleNowSupport_2")]
        public IHttpActionResult DoddleNowSupportTest()
        {
            return Ok(Order.CreateOrders());
        }

        ///<summary>
        ///Test GET request for Hospital Super Admin Role (3) only.  Returns list of fake orders
        ///</summary>
        [HttpGet]
        [Authorize(Roles = "3")]
        [Route("Roles/HospitalSuperAdmin_3")]
        public IHttpActionResult HospitalSuperAdminTest()
        {
            return Ok(Order.CreateOrders());
        }

        ///<summary>
        ///Test GET request for HR Admin Role (4) only.  Returns list of fake orders
        ///</summary>
        [HttpGet]
        [Authorize(Roles = "4")]
        [Route("Roles/HRAdmin_4")]
        public IHttpActionResult HRAdminTest()
        {
            return Ok(Order.CreateOrders());
        }

        ///<summary>
        ///Test GET request for Manager Role (5) only.  Returns list of fake orders
        ///</summary>
        [HttpGet]
        [Authorize(Roles = "5")]
        [Route("Roles/Manager_5")]
        public IHttpActionResult ManagerTest()
        {
            return Ok(Order.CreateOrders());
        }

        ///<summary>
        ///Test GET request for Healthcare Professional Role (6) only.  Returns list of fake orders
        ///</summary>
        [HttpGet]
        [Authorize(Roles = "6")]
        [Route("Roles/HealthcareProfessional_6")]
        public IHttpActionResult HealthcareProfessionalTest()
        {
            return Ok(Order.CreateOrders());
        }

        ///<summary>
        ///Test GET request for either Doddle Admin (1) or Doddle Support (2) roles.  Returns list of fake orders
        ///</summary>
        [HttpGet]
        [Authorize(Roles = "1,2")]
        [Route("Roles/DoddleNowAdminOrSupport")]
        public IHttpActionResult DoddleNowAdminOrSupportTest()
        {
            return Ok(Order.CreateOrders());
        }


    }

    #region Helpers

    ///<summary>
    ///Order entity (test only)
    ///</summary>
    public class Order
    {
        ///<summary>
        ///Order Id
        ///</summary>
        public int OrderID { get; set; }
        ///<summary>
        ///Customer name
        ///</summary>
        public string CustomerName { get; set; }
        ///<summary>
        ///Shipper city
        ///</summary>
        public string ShipperCity { get; set; }
        ///<summary>
        ///Has item shipped
        ///</summary>
        public Boolean IsShipped { get; set; }

        ///<summary>
        ///Gets a list of orders (test only)
        ///</summary>
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
