using CouponsKE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoupnsKE.Services.Web.Interfaces
{
    public interface IScraper
    {
        Task<Product> GetSingleProductAsync(string url, string htmlDocument);
        Task<List<Product>> GetMultipleProductsAsync(string url, string htmlDocument, int? productLimit, int? sellerLimit);
    }
}
