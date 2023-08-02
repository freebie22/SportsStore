﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models.ViewModels;
using Xunit;

namespace SportsStore.Models.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Can_Use_Repository()
        {
            //Організація
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product() {ProductID = 1, Name = "P1"},
                new Product() {ProductID = 2, Name = "P2"}
            }).AsQueryable());
            HomeController controller = new HomeController(mock.Object);
            //Дія
            ProductsListViewModel result = (controller.Index(null) as ViewResult).ViewData.Model as ProductsListViewModel;
            //Ствердження
            Product[] prodArray = result.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("P1", prodArray[0].Name);
            Assert.Equal("P2", prodArray[1].Name);
        }
        [Fact]
        public void Can_Paginate()
        {
            //Організація
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product() {ProductID = 1, Name = "P1"},
                new Product() {ProductID = 2, Name = "P2"},
                new Product() {ProductID = 3, Name = "P3"},
                new Product() {ProductID = 4, Name = "P4"},
                new Product() {ProductID = 5, Name = "P5"}
            }).AsQueryable());
            HomeController controller = new HomeController(mock.Object);
            controller.pageSize = 3;
            //Дія
            ProductsListViewModel result = (controller.Index(null, 2) as ViewResult).ViewData.Model as ProductsListViewModel;
            //Ствердження
            Product[] prodArray = result.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("P4", prodArray[0].Name);
            Assert.Equal("P5", prodArray[1].Name);

        }
        [Fact]
        public void Can_Filter_Products()
        {
            //Організація
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] 
            {
                new Product() {ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product() {ProductID = 2, Name = "P2", Category = "Cat2"},
                new Product() {ProductID = 3, Name = "P3", Category = "Cat1"},
                new Product() {ProductID = 4, Name = "P4", Category = "Cat2"},
                new Product() {ProductID = 5, Name = "P5", Category = "Cat3"}
            }).AsQueryable());
            HomeController controller = new HomeController(mock.Object);
            controller.pageSize = 3;
            //Дія
            Product[] result = ((controller.Index("Cat2", 1) as ViewResult).ViewData.Model as ProductsListViewModel).Products.ToArray();
            //Ствердження
            Assert.Equal(2, result.Length);
            Assert.True(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.True(result[1].Name == "P4" && result[1].Category == "Cat2");
        }

    }
}
