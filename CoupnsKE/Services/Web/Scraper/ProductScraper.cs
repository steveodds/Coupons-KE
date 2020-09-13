using CoupnsKE.Services.Web.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;
using CouponsKE.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CoupnsKE.Services.Web.Scraper
{
    public class ProductScraper : IScraper
    {
        public string HtmlDocument { get; set; }
        public int? ProductLimit { get; set; }
        public int? SellerLimit { get; set; }
        public List<string> Products { get; set; }
        public ProductScraper()
        {

        }

        public ProductScraper(string htmlDocument)
        {
            HtmlDocument = htmlDocument;
        }

        public Task<List<Product>> GetMultipleProductsAsync(string htmlDocument, int? productLimit, int? sellerLimit)
        {
            var context = BrowsingContext.New(Configuration.Default);

            return null;
        }

        public async Task<Product> GetSingleProductAsync(string htmlDocument)
        {
            var product = new Product();

            //AngelSharp prep
            var context = BrowsingContext.New(Configuration.Default);
            var source = htmlDocument;
            var document = await context.OpenAsync(req => req.Content(source));
            var result = document.DocumentElement.OuterHtml;

            var jsonFromHtml = document.All.Where(m => m.LocalName == "script" && m.GetAttribute("type") == "application/ld+json").FirstOrDefault().TextContent;
            var sku = document.All.Where(m => m.LocalName == "a" && m.GetAttribute("id") == "wishlist").FirstOrDefault().GetAttribute("data-simplesku");
            sku = sku.Substring(0, sku.IndexOf('-')).Trim();
            var category = document.All.Where(m => m.LocalName == "form" && m.GetAttribute("data-id") == sku).FirstOrDefault().GetAttribute("data-category");
            product = GetJSONElement(jsonFromHtml);
            product.SKU = sku;
            product.ProductCategory = category;

            return product;
        }

        private Product GetJSONElement(string jsonText)
        {
            dynamic structuredJson = JObject.Parse(jsonText);
            var product = new Product
            {
                Price = structuredJson.mainEntity.offers.price,
                ProductDescription = structuredJson.mainEntity.description,
                ProductName = structuredJson.mainEntity.name,
                ImageUrl = structuredJson.mainEntity.image.contentUrl[0]
            };
            return product;
        }
    }


    public class WebPage
    {
        public string Url { get; set; }
        public string HtmlDocument { get; set; }
    }
}
