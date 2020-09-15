using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CouponsKE.Models
{
    public class Product
    {
        public Guid ProductID { get; set; }
        [Display(Name = "Product")]
        public string ProductName { get; set; }
        [Display(Name = "Category")]
        public string ProductCategory { get; set; }
        [Display(Name = "Description")]
        public string ProductDescription { get; set; }
        public string ImageUrl { get; set; }
        public string StoreName { get; set; }
        public string SKU { get; set; }
        [Display(Name ="Referral Link")]
        public string StoreLink { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
    }
}
