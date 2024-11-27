using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicStore.Domain.Domain;
using MusicStore.Repository;
using MusicStore.Service.Interface;
using Stripe;

namespace MusicStore.Web.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartsController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        // GET: ShoppingCarts
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.LoggedIn = userId != null;

            var dto = _shoppingCartService.getShoppingCartInfo(userId);
            return View(dto);
        }

        public IActionResult DeleteFromShoppingCart(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _shoppingCartService.deleteProductFromShoppingCart(userId, id);

            return RedirectToAction("Index");

        }


        public IActionResult order()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.LoggedIn = userId != null;

            var result = _shoppingCartService.order(userId);
            //if (result == true)
            return RedirectToAction("Index", "ShoppingCarts");


        }

        public IActionResult SuccessPayment()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ViewBag.LoggedIn = userId != null;

            return View();
        }

        public IActionResult PayOrder(string stripeEmail, string stripeToken)
        {
            var customerService = new CustomerService();
            var chargeService = new ChargeService();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ViewBag.LoggedIn = userId != null;

            var order = this._shoppingCartService.getShoppingCartInfo(userId);

            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount = (Convert.ToInt32(order.TotalPrice) * 100),
                Description = "Payment",
                Currency = "usd",
                Customer = customer.Id
            });

            if (charge.Status == "succeeded")
            {
                this.order();
                return RedirectToAction("SuccessPayment");
            }
            else
            {
                return RedirectToAction("NotsuccessPayment");
            }
        }


    }
}
