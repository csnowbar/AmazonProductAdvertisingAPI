using AmazonProductAdvertisingAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ServiceModel;
using AmazonProductAdvertisingAPI.Domain.esc;
using System.Diagnostics;
using System.Xml.Serialization;
namespace AmazonProductAdvertisingAPI.Domain.Amazon
{
    public class AmazonRequestHelper
    {
        private const string accessKeyId = "AKIAJ4277VFFZSMHQRQA";
        private const string secretKey = "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";
        private const string soapUrl = "https://webservices.amazon.com/onca/soap?Service=AWSECommerceService";
        
        public ItemSearchResponse ItemSearch(string ItemPage, string SearchIndex, string[] ResponseGroup, string Title)
        {
            // BasicHttpBinding configuration
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;

            // create a WCF Amazon ECS client
            AWSECommerceServicePortTypeClient client = new AWSECommerceServicePortTypeClient(
                binding,
                new EndpointAddress("https://webservices.amazon.com/onca/soap?Service=AWSECommerceService"));

            // add authentication to the ECS client
            client.ChannelFactory.Endpoint.Behaviors.Add(new AmazonSigningEndpointBehavior(accessKeyId, secretKey));

            // prepare an ItemSearchRequest
            ItemSearchRequest request = new ItemSearchRequest();
            request.SearchIndex = SearchIndex;
            request.ResponseGroup = ResponseGroup;
            request.Title = Title;
            request.ItemPage = ItemPage;

            // prepare an ItemSearch
            ItemSearch search = new ItemSearch();
            search.Request = new ItemSearchRequest[] { request };
            search.AWSAccessKeyId = accessKeyId;
            search.AssociateTag = "http://affiliate-program.amazon.com/";

            //get ItemSearchResponse
            ItemSearchResponse response = client.ItemSearch(search);

            return response;
        }

        
        public static ProductsPages ItemSearchRequestResult(string title, int webPage, int itemPerPage)
        {
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;

            // create a WCF Amazon ECS client
            AWSECommerceServicePortTypeClient client = new AWSECommerceServicePortTypeClient(
                binding,
                new EndpointAddress("https://webservices.amazon.com/onca/soap?Service=AWSECommerceService"));

            // add authentication to the ECS client
            client.ChannelFactory.Endpoint.Behaviors.Add(new AmazonSigningEndpointBehavior(accessKeyId, secretKey));

            // prepare an ItemSearch request

            ItemSearchRequest searchRequest = new ItemSearchRequest();
            searchRequest.SearchIndex = "Books";
            searchRequest.Title = title;
            searchRequest.ResponseGroup = new string[] { "ItemAttributes", "Offers" };

            ItemSearch itemSearch = new ItemSearch();
            itemSearch.Request = new ItemSearchRequest[] { searchRequest };
            itemSearch.AWSAccessKeyId = accessKeyId;
            itemSearch.AssociateTag = "http://affiliate-program.amazon.com/";

            int skip = (webPage - 1) * itemPerPage;
            int startPage;
            if ((skip % 10) <= 2 && (skip % 10) != 0)
            {
                startPage = webPage + 1;
            }
            else
            {
                startPage = webPage;
            }
            int startItem = skip % 10;
            ProductsPages productPages = new ProductsPages();
            
            for (int page = startPage; page <= 10; page++)
            {
                searchRequest.ItemPage = Convert.ToString(page);
                ItemSearchResponse searchResponse = client.ItemSearch(itemSearch);
                int totalPages = Convert.ToInt32(searchResponse.Items[0].TotalPages);
                if(totalPages < 10)
                {
                    productPages.Pages = totalPages;
                }
                else
                {
                    productPages.Pages = 10;
                }
                if (searchResponse.Items[0].Item != null)
                {
                    for (int i = startItem; i < searchResponse.Items[0].Item.Length; i++)
                    {
                        if (itemPerPage > 0)
                        {
                            var item = searchResponse.Items[0].Item[i];

                            Debug.WriteLine("{0}\t{1}\tpage:{2}\titem:{3}", item.ItemAttributes.Title, Convert.ToDecimal(item.OfferSummary.LowestNewPrice.Amount) / 100, page, i);
                            Product currentProduct = new Product()
                            {
                                Title = item.ItemAttributes.Title,
                                Price = Convert.ToDecimal(item.OfferSummary.LowestNewPrice.Amount) / 100
                            };
                            productPages.Products.Add(currentProduct);
                            itemPerPage--;
                        }
                        else
                            break;

                    }
                    startItem = 0;
                    Thread.Sleep(1000); //Amazon can ask requests only once per second

                }
            }
            return productPages;
        }
    }
}
