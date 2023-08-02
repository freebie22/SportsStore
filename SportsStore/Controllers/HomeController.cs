using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using System.Diagnostics;

namespace SportsStore.Controllers
{
    public class HomeController : Controller
    {
        private IStoreRepository repository;
        public int pageSize = 4;
        public HomeController(IStoreRepository repository)
        {
            this.repository = repository;
        }

        public IActionResult Index(string category,int productPage = 1)
        {
            return View(new ProductsListViewModel
            { 
                Products = repository.Products.Where(p => category == null || p.Category == category).OrderBy(p => p.ProductID).Skip((productPage - 1) * pageSize).Take(pageSize).ToList(),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ? repository.Products.Count() : repository.Products.Where(e => e.Category == category).Count()
                }
            } ); 
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}