using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;

namespace QrTailor.Application.Services.CrawlService.TrendyolService
{
    internal class TrendyolCrawlManager : ITrendyolCrawlService
    {
        public async Task<List<CrawlModel>> GetTrendyolProductsAsync(string searchText)
        {
            string website = "https://www.trendyol.com/sr?q=";
            string url = String.Format("{0}{1}", website, GenerateSearchText(searchText));
            var htmlContent = await FetchHtmlContent(url);
            return ParseHtmlContent(htmlContent);
        }

        private static async Task<string> FetchHtmlContent(string url)
        {
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli
            };

            using var client = new HttpClient(handler);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
            client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("tr-TR,tr;q=0.9");
            //client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip, deflate, br");

            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var pageContents = await response.Content.ReadAsStringAsync();
            return pageContents;
        }
        private static List<CrawlModel> ParseHtmlContent(string htmlContent)
        {
            List<CrawlModel> result = new List<CrawlModel>();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);

            var products = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'product')]");

            //Console.WriteLine(node.InnerText.Trim());

            if (products != null)
            {
                foreach (var product in products)
                {
                    
                    if (product != null)
                    {
                        string? Title = product.InnerText.Trim();
                        string? Link = "-";
                        string rating = "-";
                        string? Price = "-";

                        result.Add(new CrawlModel
                        {
                            ProductName = Title,
                            ProductLink = Link,
                            ProductImageLink = "-",
                            ProductPrice = Price,
                        });
                    }
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
