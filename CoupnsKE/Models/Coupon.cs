using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CouponsKE.Models
{
    public class Coupon
    {
        public Guid CouponID { get; set; }
        public string Store { get; set; }

        [Display(Name = "Category")]
        public string CouponCategory { get; set; }

        [Display(Name = "Coupon")]
        public string CouponCode { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Expires on")]
        [DataType(DataType.Date)]
        public DateTime ExpiryDate { get; set; }

        [Display(Name = "T&Cs")]
        public string Restrictions { get; set; }

        [Display(Name = "Link")]
        public string CouponUrl { get; set; }

        [Display(Name = "Seller")]
        public string? StoreSeller { get; set; }
    }
}
