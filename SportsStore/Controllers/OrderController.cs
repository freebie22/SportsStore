using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository repository;
        private Cart cart;
        public OrderController(IOrderRepository repository, Cart cart)
        {
            this.repository = repository;
            this.cart = cart;
        }
        [HttpGet]
        public IActionResult Checkout() => View(new Order());

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if(cart.Lines.Count == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }

            if(order.Line2 == null)
            {
                order.Line2 = "Не вказано";
            }

            if(order.Line3 == null)
            {
                order.Line3 = "Не вказано";
            }

            if(ModelState.IsValid)
            {
                order.Lines = cart.Lines.ToArray();
                repository.SaveOrder(order);
                cart.Clear();
                return RedirectToPage("/Completed", new { orderId = order.OrderID });
            }

            else
            {
                return View(order);
            }
        }
    }
}
