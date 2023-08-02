using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using Xunit;

namespace SportsStore.Models.Tests
{
    public class OrderControllerTests
    {
        [Fact]
        public void Cannot_Checkout_Empty_Cart()
        {
            //Організація - створення імітованого сховища даних
            Mock<IOrderRepository> mock= new Mock<IOrderRepository>();
            //Організація - створення порожнього кошика
            Cart cart = new Cart();
            //Організація створення нового замовлення
            Order order = new Order();
            //Організація - створення контролера
            OrderController target = new OrderController(mock.Object, cart);
            //Дія
            ViewResult result = target.Checkout(order) as ViewResult;
            //Ствердження - перевірка, що замовлення не було збережено
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            //Ствердження - перевірка, що метод повертає стандартне представлення
            Assert.True(string.IsNullOrEmpty((result.ViewName)));
            //Ствердження - перевірка, що представленню передана неприпустима модель
            Assert.False(result.ViewData.ModelState.IsValid);
        }
    }
}
