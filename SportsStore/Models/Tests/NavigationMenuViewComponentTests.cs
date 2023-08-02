using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using SportsStore.Components;
using SportsStore.Controllers;
using SportsStore.Models.ViewModels;
using Xunit;

namespace SportsStore.Models.Tests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void Can_Select_Categories()
        {
            //Організація
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Apples"},
                new Product {ProductID = 2, Name = "P2", Category = "Apples"},
                new Product {ProductID = 3, Name = "P3", Category = "Plums"},
                new Product {ProductID = 4, Name = "P4", Category = "Oranges"},
            }).AsQueryable());
            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);
            //Дія - отримання набору категорій
            string[] results = ((IEnumerable<string>)(target.Invoke() as ViewViewComponentResult).ViewData.Model).ToArray();

            //Ствердження
            Assert.True(Enumerable.SequenceEqual(new string[] {"Apples", "Oranges", "Plums"}, results));

        }
        [Fact]
        public void Indicates_Selected_Category()
        {
            //Організація
            string categoryToSelect = "Apples";
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Apples"},
                new Product {ProductID = 4, Name = "P4", Category = "Oranges"},
            }).AsQueryable());
            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);
            target.ViewComponentContext = new ViewComponentContext
            {
                ViewContext = new ViewContext
                {
                    RouteData = new Microsoft.AspNetCore.Routing.RouteData()
                }
            };
            target.RouteData.Values["category"] = categoryToSelect;
            //Дія
            string result = (string)(target.Invoke() as ViewViewComponentResult).ViewData["SelectedCategory"];
            Assert.Equal(categoryToSelect, result);
        }

        [Fact]
        public void Generate_Category_Specific_Product_Count()
        {
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
                new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
                new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductID = 5, Name = "P5", Category = "Cat3"},
            }).AsQueryable());
            HomeController controller = new HomeController(mock.Object);
            controller.pageSize = 3;
            Func<ViewResult, ProductsListViewModel> GetModel = result => result?.ViewData?.Model as ProductsListViewModel;

            int? res1 = GetModel(controller.Index("Cat1") as ViewResult)?.PagingInfo.TotalItems;
            int? res2 = GetModel(controller.Index("Cat2") as ViewResult)?.PagingInfo.TotalItems;
            int? res3 = GetModel(controller.Index("Cat3") as ViewResult)?.PagingInfo.TotalItems;
            int? resAll = GetModel(controller.Index(null) as ViewResult)?.PagingInfo.TotalItems;

            Assert.Equal(2, res1);
            Assert.Equal(2, res2);
            Assert.Equal(1, res3);
            Assert.Equal(5, resAll);
        }
    }
}
