using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CouponsKE.Models
{
    public class Store
    {
        public Guid StoreID { get; set; }
        [Display(Name ="Store")]
        public string StoreName { get; set; }
        [Display(Name ="Beginning of referral link")]
        public string StoreReflinkStart { get; set; }
        
        [Display(Name ="End of referral link")]
        public string StoreReflinkEnd { get; set; }
    }
}
