using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmazonProductAdvertisingAPI.Domain.Concrete;

namespace AmazonProductAdvertisingAPI.WebUI.Infrastructure.Factories
{
    public interface IAmazonProductCollectionFactory
    {
        AmazonProductCollection Create(string keywords, int pageNumber, int itemsPerPage);
    }
}
