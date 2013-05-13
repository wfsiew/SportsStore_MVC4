using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public int PageSize = 4;

        public ProductController(IProductRepository productRepository)
        {
            repository = productRepository;
        }

        //
        // GET: /Product/

        public ActionResult Index()
        {
            return View();
        }

        public ViewResult List(string category, int page = 1)
        {
            ProductsListViewModel viewModel = new ProductsListViewModel
            {
                Products = repository.Products
                .Where(delegate(Product p)
                {
                    return (category == null || p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
                })
                .OrderBy(delegate(Product p)
                {
                    return p.ProductID;
                })
                .Skip((page - 1) * PageSize)
                .Take(PageSize),
                PagingInfo = new PagingInfo {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ? 
                        repository.Products.Count() :
                        repository.Products.Where(delegate(Product e)
                        {
                            return e.Category.Equals(category, StringComparison.OrdinalIgnoreCase);
                        }).Count()
                },
                CurrentCategory = category
            };

            return View(viewModel);
        }
    }
}
