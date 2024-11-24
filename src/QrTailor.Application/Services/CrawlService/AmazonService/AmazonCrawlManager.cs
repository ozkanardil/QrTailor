
using HtmlAgilityPack;
using static System.Net.WebRequestMethods;

namespace QrTailor.Application.Services.CrawlService.AmazonService
{
    public class AmazonCrawlManager : IAmazonCrawlService
    {
        public List<CrawlModel> GetAmazonDummyProducts()
        {
            List<CrawlModel> result =
            [
                new CrawlModel { ProductName = "iPhone", ProductLink = "https://www.google.de", ProductImageLink="", ProductPrice="10" },
                new CrawlModel { ProductName = "Samsung", ProductLink = "https://www.samsung.com", ProductImageLink="", ProductPrice="20" },
            ];
            return result;
        }

        public async Task<List<CrawlModel>> GetAmazonProductsAsync(string searchText)
        {
            List<CrawlModel> result = new List<CrawlModel>();

            // 1. URL
            string website = "https://www.google.de/search?tbm=shop&hl=en&q=";
            string url = String.Format("{0}{1}", website, GenerateSearchText(searchText));

            // 2. HttpClient ile sayfayı çek
            HttpClient httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            // 3. HtmlAgilityPack ile HTML'yi parse et
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            //var nodes = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'rgHvZc')]");
            var products = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'P8xhZc')]");

            // 4. Link başlıklarını bul
            //var productTitles = htmlDoc.DocumentNode.SelectNodes("//h3[@class='sh-np__product-title']");

            if (products != null)
            {
                foreach (var product in products)
                {
                    // Başlık metnini al
                    var title = product.SelectSingleNode(".//a");
                    var ratingNode = product.SelectSingleNode(".//div[contains(@class, 'm0amQc')]");
                    var priceNode = product.SelectSingleNode(".//span[contains(@class, 'HRLxBb')]");
                    //var reviewCountNode = product.SelectSingleNode(".//span[contains(@class, 'review-count')]");
                    //var imageNode = product.SelectSingleNode(".//img[contains(@class, 'product-image')]");

                    string? Title = title.InnerText.Trim();
                    string? Link = title.GetAttributeValue("href", string.Empty);
                    string rating = "-";
                    if (ratingNode != null && ratingNode.InnerHtml.IndexOf("aria-label") > -1)
                    {
                        rating = ratingNode.GetAttributeValue("aria-label", string.Empty);
                        var ratingString = rating.Split(' ')[0];
                    }
                    string? Price = priceNode.InnerText.Trim();
                    //ReviewCount = reviewCountNode != null ? int.Parse(reviewCountNode.InnerText.Split(' ')[0]) : (int?)null,

                    result.Add(new CrawlModel
                    {
                        ProductName = Title,
                        ProductLink = Link,
                        ProductImageLink = "-",
                        ProductPrice = Price,
                    });
                }
            }
            return result;
        }

        private string GenerateSearchText(string searchText)
        {
            searchText = searchText.Trim();
            searchText = searchText.Replace("    ", " ");
            searchText = searchText.Replace("   ", " ");
            searchText = searchText.Replace("  ", " ");
            searchText = searchText.Replace(" ", "+");
            return searchText;
        }
    }
}
