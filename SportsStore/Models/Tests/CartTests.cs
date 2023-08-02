using Xunit;

namespace SportsStore.Models.Tests
{
    public class CartTests
    {
        [Fact]
        public void Can_Add_New_Lines()
        {
            //Організація - створення декількох тестових товарів
            Product p1 = new Product() { ProductID = 1, Name = "P1" };
            Product p2 = new Product() { ProductID = 2, Name = "P2" };
            //Організація - створення нового кошика
            Cart target = new Cart();
            //Дія
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            CartLine[] results = target.Lines.ToArray();
            //Очікування
            Assert.Equal(2, results.Length);
            Assert.Equal(p1, results[0].Product);
            Assert.Equal(p2, results[1].Product);
        }
        [Fact]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            //Організація - створення декількох тестових товарів
            Product p1 = new Product() { ProductID = 1, Name = "P1" };
            Product p2 = new Product() { ProductID = 2, Name = "P2" };
            //Організація - створення нового кошика
            Cart target = new Cart();
            //Дія
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 10);
            CartLine[] results = target.Lines.OrderBy(r => r.Product.ProductID).ToArray();
            //Очікування
            Assert.Equal(2, results.Length);
            Assert.Equal(11, results[0].Quantity);
            Assert.Equal(1, results[1].Quantity);
        }
        [Fact]
        public void Can_Remove_Lines()
        {
            //Організація - створення декількох тестових товарів
            Product p1 = new Product() { ProductID = 1, Name = "P1" };
            Product p2 = new Product() { ProductID = 2, Name = "P2" };
            Product p3 = new Product() { ProductID = 3, Name = "P3" };
            //Організація - створення нового кошика
            Cart target = new Cart();
            //Дія
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p3, 1);
            target.AddItem(p2, 10);
            target.RemoveLine(p2);
            //Очікування
            Assert.Empty(target.Lines.Where(c => c.Product == p2));
            Assert.Equal(2, target.Lines.Count());
        }
        [Fact]
        public void Calculate_Cart_Total()
        {
            //Організація - створення декількох тестових товарів
            Product p1 = new Product() { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product() { ProductID = 2, Name = "P2", Price = 50M };
            //Організація - створення нового кошика
            Cart target = new Cart();
            //Дія
            target.AddItem(p1, 3);
            target.AddItem(p2, 3);
            //Очікування
            Assert.Equal(450M, target.ComputeTotalValue());
        }
        [Fact]
        public void Can_Clear_Contents()
        {
            //Організація - створення декількох тестових товарів
            Product p1 = new Product() { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product() { ProductID = 2, Name = "P2", Price = 50M };
            //Організація - створення нового кошика
            Cart target = new Cart();
            //Дія
            target.AddItem(p1, 3);
            target.AddItem(p2, 3);
            target.Lines.Clear();
            //Очікування
            Assert.Empty(target.Lines);
        }

    }
}
