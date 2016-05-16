using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AmazonProductAdvertisingAPI.Domain.Abstract;
using AmazonProductAdvertisingAPI.WebUI.Models;
using AmazonProductAdvertisingAPI.WebUI.Infrastructure.Factories;

using AmazonProductAdvertisingAPI.Domain.Entities;
namespace AmazonProductAdvertisingAPI.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductCollection _productCollection;
        private IAmazonProductCollectionFactory _productCollectionFactory;
        private ProductListViewModel _model;
        public int pageSize = 13;
        
        public ProductController(IAmazonProductCollectionFactory productCollectionFactory)
        {
            _productCollectionFactory = productCollectionFactory;
        }

        public static HttpContext Current { get; set; }


        // GET: Product
        [OutputCache(Duration = 3600, VaryByParam = "none")]
        public ActionResult List(int page = 1, string search = "")
        {
            Response.Cache.SetCacheability(HttpCacheability.Public);


            Response.Cache.SetExpires(DateTime.Now.AddSeconds(600));


            Response.Cache.SetValidUntilExpires(true);

            if (search != "")
            {
                this.Session["search"] = search;
            }
            
            _productCollection = _productCollectionFactory.Create((string)this.Session["search"], page, 13);
            
            _model = new ProductListViewModel()
            {
                Products = _productCollection.ProductInfo.Products,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalPages = _productCollection.ProductInfo.Pages
                }
            };
            return View(_model);
        }

        public ActionResult CurrencyConverter(string toCurrency)
        {
            var currencyRates = new AmazonProductAdvertisingAPI.Domain.OpenExchangeRates.CurrencyRatesRequestHelper();

            decimal rate = currencyRates.getCurrencyRates.Rates[toCurrency];

            return Content(rate.ToString(), "text/plain");
        }
    }
}