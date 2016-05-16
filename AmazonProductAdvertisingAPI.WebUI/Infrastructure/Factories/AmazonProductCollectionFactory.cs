using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmazonProductAdvertisingAPI.Domain.Concrete;
using Ninject.Syntax;
using Ninject.Parameters;
using Ninject;

namespace AmazonProductAdvertisingAPI.WebUI.Infrastructure.Factories
{
    
    public class AmazonProductCollectionFactory : IAmazonProductCollectionFactory
    {
        public readonly IResolutionRoot m_ResolutionRoot;
        public AmazonProductCollectionFactory(IResolutionRoot resolution_root)
        {
            m_ResolutionRoot = resolution_root;
        }
        public AmazonProductCollection Create(string keywords, int pageNumber, int itemsPerPage)
        {
            return m_ResolutionRoot.Get<AmazonProductCollection>(
                new ConstructorArgument("keywords", keywords),
                new ConstructorArgument("pageNumber", pageNumber),
                new ConstructorArgument("itemsPerPage", itemsPerPage));
        }
    }
}
