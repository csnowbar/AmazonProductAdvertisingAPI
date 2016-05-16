using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmazonProductAdvertisingAPI.Domain.Abstract;
using AmazonProductAdvertisingAPI.Domain.Entities;
using AmazonProductAdvertisingAPI.Domain.Amazon;
using AmazonProductAdvertisingAPI.Domain.esc;
using System.Threading;
using System.Diagnostics;

namespace AmazonProductAdvertisingAPI.Domain.Concrete
{
    public class AmazonProductCollection : IProductCollection
    {
        private string _keywords;
        private int _pageNumber;
        private int _itemsPerPage;
        private ProductsPages _productInfo;
        public ProductsPages ProductInfo
        {
            get { return _productInfo; }
        }
        public AmazonProductCollection(string keywords, int pageNumber, int itemsPerPage)
        {
            _keywords = keywords;
            _pageNumber = pageNumber;
            _itemsPerPage = itemsPerPage;
            this.ProductsInfo();
        }

        public ProductsPages ProductsPages
        {
            get
            {
                return AmazonRequestHelper.ItemSearchRequestResult(_keywords, _pageNumber, _itemsPerPage);
            }
        }

        public void ProductsInfo()
        {
            ProductsPages productsInfo = new ProductsPages();

            


            int skip = (_pageNumber - 1) * _itemsPerPage;

            int startPage;
            if ((skip % 10) <= 2 && (skip % 10) != 0)
            {
                startPage = _pageNumber + 1;
            }
            else
            {
                startPage = _pageNumber;
            }

            int startItem = skip % 10;


            ///////////////////

            for (int page = startPage; page <= 10; page++)
            {
                AmazonRequestHelper amazonRequestHelper = new AmazonRequestHelper();
                string itemPage = Convert.ToString(page);
                ItemSearchResponse itemSearchResponse = amazonRequestHelper.ItemSearch(itemPage, "Books", new string[] { "ItemAttributes", "Offers" }, _keywords);

                int totalPages = Convert.ToInt32(itemSearchResponse.Items[0].TotalPages);

                if (totalPages >= 10)
                    productsInfo.Pages = 10;
                else
                    productsInfo.Pages = totalPages;


                if (itemSearchResponse.Items[0].Item != null)
                {
                    for (int i = startItem; i < itemSearchResponse.Items[0].Item.Length; i++)
                    {
                        if (_itemsPerPage > 0)
                        {
                            var item = itemSearchResponse.Items[0].Item[i];

                            Debug.WriteLine("{0}\t{1}\tpage:{2}\titem:{3}", item.ItemAttributes.Title, Convert.ToDecimal(item.OfferSummary.LowestNewPrice.Amount) / 100, page, i);
                            Product currentProduct = new Product()
                            {
                                Title = item.ItemAttributes.Title,
                                Price = Convert.ToDecimal(item.OfferSummary.LowestNewPrice.Amount) / 100
                            };
                            productsInfo.Products.Add(currentProduct);
                            _itemsPerPage--;
                        }
                        else
                            break;

                    }
                    startItem = 0;
                    Thread.Sleep(1000);

                }

            }
            _productInfo = productsInfo;
        }
    }
}
