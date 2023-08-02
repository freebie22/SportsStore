using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SportsStore.Models.ViewModels;
using Moq;
using SportsStore.Infrastructure;
using Xunit;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SportsStore.Controllers;

namespace SportsStore.Models.Tests
{
    public class PageLinkTagHelperTests
    {
        [Fact]
        public void Can_Generate_Page_Links()
        {
            //Організація
            Mock<IUrlHelper> urlHelper = new Mock<IUrlHelper>();
            urlHelper.SetupSequence(x => x.Action(It.IsAny<UrlActionContext>())).Returns("Test/Page1").Returns("Test/Page2").Returns("Test/Page3");

            var urlHelperFactory = new Mock<IUrlHelperFactory>();
            urlHelperFactory.Setup(f=> f.GetUrlHelper(It.IsAny<ActionContext>())).Returns(urlHelper.Object);

            PageLinkTagHelper helper = new PageLinkTagHelper(urlHelperFactory.Object)
            {
                PageModel = new PagingInfo {
                    CurrentPage = 2,
                    TotalItems = 28,
                    ItemsPerPage = 10,
                },
                PageAction = "Test"
            };

            TagHelperContext ctx = new TagHelperContext(new TagHelperAttributeList(), new Dictionary<object, object>(), "");
            var content = new Mock<TagHelperContent>();
            TagHelperOutput output = new TagHelperOutput("div", new TagHelperAttributeList(), (cache, encoder) => Task.FromResult(content.Object));
            //Дія
            helper.Process(ctx, output);
            //Ствердження
            Assert.Equal(@"<a href=""Test/Page1"">1</a>"
            + @"<a href=""Test/Page2"">2</a>"
            + @"<a href=""Test/Page3"">3</a>",
            output.Content.GetContent()); 

        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            //Організація
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] 
            {
                new Product() {ProductID = 1, Name = "P1"},
                new Product() {ProductID = 2, Name = "P2"},
                new Product() {ProductID = 3, Name = "P3"},
                new Product() {ProductID = 4, Name = "P4"},
                new Product() {ProductID = 5, Name = "P5"},
            }).AsQueryable());
            HomeController homeController = new HomeController(mock.Object) {pageSize = 3};
            //Дія
            ProductsListViewModel result = (homeController.Index(null, 2) as ViewResult).ViewData.Model as ProductsListViewModel;
            //Ствердження
            PagingInfo pagingInfo = result.PagingInfo;
            Assert.Equal(2, pagingInfo.CurrentPage);
            Assert.Equal(3, pagingInfo.ItemsPerPage);
            Assert.Equal(5, pagingInfo.TotalItems);
            Assert.Equal(2, pagingInfo.TotalPages);
        }
    }
}
