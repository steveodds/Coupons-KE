using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CouponsKE.Models
{
    public class Deals
    {
        public Guid DealsID { get; set; }

        [Display(Name = "Deal")]
        public string DealName { get; set; }

        [Display(Name = "Description")]
        public string DealDescription { get; set; }

        [Display(Name = "Rating")]
        public float DealRating { get; set; }
        public string Store { get; set; }

        [Display(Name = "Old Price")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal OldPrice { get; set; }

        [Display(Name = "New Price")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal NewPrice { get; set; }
    }
}
