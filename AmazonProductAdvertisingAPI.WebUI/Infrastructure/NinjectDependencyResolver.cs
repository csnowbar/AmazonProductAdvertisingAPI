using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using System.Web.Mvc;
using AmazonProductAdvertisingAPI.Domain.Abstract;
using AmazonProductAdvertisingAPI.Domain.Entities;
using Moq;
using AmazonProductAdvertisingAPI.Domain.Amazon;
using AmazonProductAdvertisingAPI.Domain.Concrete;
using AmazonProductAdvertisingAPI.WebUI.Controllers;
using AmazonProductAdvertisingAPI.WebUI.Infrastructure.Factories;
namespace AmazonProductAdvertisingAPI.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            // Create dependency here

            Mock<IProductCollection> mock = new Mock<IProductCollection>();
            //mock.Setup(m => m.Products).Returns(new List<Product>
            //{
            //    new Product { Title = "Book1", Price = 100 },
            //    new Product { Title = "Book2", Price = 200 },
            //    new Product { Title = "Book3", Price = 300 }
            //});

            //mock.Setup(m => m.Products).Returns(AmazonRequestHelper.ItemSearchRequestResult("Bible"));

            //kernel.Bind<IProductCollection>().ToConstant(mock.Object);
            //kernel.Bind<IProductCollection>().To<AmazonProductCollection>()
            //                                 .WhenInjectedInto<ProductController>()
            //                                 .WithConstructorArgument("title", ProductController.Title)
            //                                 .WithConstructorArgument("pageNumber", ProductController.PageNumber)
            //                                 .WithConstructorArgument("itemsPerPage", ProductController.ItemsPerPage);
            kernel.Bind<IAmazonProductCollectionFactory>().To<AmazonProductCollectionFactory>()
                                             .WhenInjectedInto<ProductController>();
        }

    }
}