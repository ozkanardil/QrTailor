using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QrTailor.Application.Services.CrawlService
{
    public record CrawlModel
    {
        public string? ProductName { get; set; }
        public string? ProductLink { get; set; }
        public string? ProductImageLink { get; set; }
        public string? ProductPrice { get; set; }
    }
}
