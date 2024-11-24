using QrTailor.Application.Services.CrawlService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QrTailor.Application.Services.CrawlService.AmazonService
{
    public interface IAmazonCrawlService
    {
        List<CrawlModel> GetAmazonDummyProducts();
        Task<List<CrawlModel>> GetAmazonProductsAsync(string searchText);
    }
}
