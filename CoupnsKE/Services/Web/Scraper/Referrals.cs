using CoupnsKE.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoupnsKE.Services.Web.Scraper
{
    public class Referrals
    {
        private enum Store
        {
            Jumia,
            Masoko,
            Kilimall
        }
        public string ReferralUrl { get; set; }
        public string OriginalUrl { get; set; }
        private readonly ApplicationDbContext _context;
        public Referrals(string url, ApplicationDbContext context)
        {
            OriginalUrl = url;
            _context = context;
        }

        public string ReferralLink()
        {
            var refWrappers = GetLinkWrappers(OriginalUrl);
            var modifiedURL = OriginalUrl.Replace(@"/","%2F");
            modifiedURL = modifiedURL.Replace(@":", "%3A");
            return $"{refWrappers[0]}{modifiedURL}{refWrappers[1]}";
        }

        private string[] GetLinkWrappers(string url)
        {
            var store = LinkStore(url);
            if (store is null)
                return null;

            var storeRefStart = _context.Store.FirstOrDefault(m => m.StoreName == store.ToString()).StoreReflinkStart;
            var storeRefEnd = _context.Store.FirstOrDefault(m => m.StoreName == store.ToString()).StoreReflinkEnd;
            return new string[] { storeRefStart, storeRefEnd};
        }

        private Store? LinkStore(string url)
        {
            var stores = System.Enum.GetValues(typeof(Store));
            foreach (var store in stores)
            {
                if (url.Contains(store.ToString().ToLower()))
                {
                    System.Enum.TryParse(store.ToString(), out Store result);
                    return result;
                }
            }
            return null;
        }
    }


}
