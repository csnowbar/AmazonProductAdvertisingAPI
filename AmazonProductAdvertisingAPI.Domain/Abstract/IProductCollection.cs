using AmazonProductAdvertisingAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonProductAdvertisingAPI.Domain.Abstract
{
    public interface IProductCollection
    {
        ProductsPages ProductsPages { get; } 
        ProductsPages ProductInfo { get; }
    }
}
