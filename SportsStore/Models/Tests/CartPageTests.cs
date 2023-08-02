using Moq;
using SportsStore.Pages;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace SportsStore.Models.Tests
{
    public class CartPageTests
    {
        [Fact]
        public void Can_Load_Cart()
        {
            //Організація - створення імітованого сховища даних
            Product p1 = new Product() { ProductID = 1, Name = "P1" };
            Product p2 = new Product() { ProductID = 2, Name = "P2" };
            Mock<IStoreRepository> mockRepo = new Mock<IStoreRepository>();
            mockRepo.Setup(m => m.Products).Returns((new Product[] { p1, p2 }).AsQueryable());
            //Організація - створення кошику
            Cart cart = new Cart();
            cart.AddItem(p1, 2);
            cart.AddItem(p2, 1);
            ////Організація - створення імітованого контексту сторінки і сеансу
            //Mock<ISession> mockSession = new Mock<ISession>();
            //byte[] data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(cart));
            //mockSession.Setup(c => c.TryGetValue(It.IsAny<string>(), out data));
            //Mock<HttpContext> mockContext= new Mock<HttpContext>();
            //mockContext.SetupGet(c => c.Session).Returns(mockSession.Object);
            //Дія
            CartModel cartModel = new CartModel(mockRepo.Object, cart);
            cartModel.OnGet("myUrl");
            //Ствердження
            Assert.Equal(2, cartModel.Cart.Lines.Count);
            Assert.Equal("myUrl", cartModel.ReturnUrl);

        }
        [Fact]
        public void Can_Update_Cart()
        {
            //Організація - створення імітованого сховища даних
            Mock<IStoreRepository> mockRepo= new Mock<IStoreRepository>();
            mockRepo.Setup(m => m.Products).Returns((new Product[] {new Product() { ProductID = 1, Name = "P1"} }).AsQueryable());
            Cart cart = new Cart();
            CartModel cartModel = new CartModel(mockRepo.Object, cart);
            cartModel.OnPost(1, "MyUrl");
            //Ствердження
            Assert.Single(cart.Lines);
            Assert.Equal("P1", cart.Lines.First().Product.Name);
            Assert.Equal(1, cart.Lines.First().Product.ProductID);
        }
    }
}
