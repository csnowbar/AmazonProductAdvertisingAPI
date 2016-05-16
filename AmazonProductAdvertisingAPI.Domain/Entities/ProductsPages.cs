using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonProductAdvertisingAPI.Domain.Entities
{
    public class ProductsPages
    {
        private List<Product> _products = new List<Product>();
        private int _pages;
        public List<Product> Products
        {
            set
            {
                _products = value;
            }
            get
            {
                return _products;
            }
        }
        public int Pages { get; set; }
    }
}
