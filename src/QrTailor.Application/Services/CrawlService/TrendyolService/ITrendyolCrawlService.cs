using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QrTailor.Application.Services.CrawlService.TrendyolService
{
    public interface ITrendyolCrawlService
    {
        Task<List<CrawlModel>> GetTrendyolProductsAsync(string searchText);
    }
}
