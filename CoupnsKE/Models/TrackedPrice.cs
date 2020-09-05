using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CouponsKE.Models
{
    public class TrackedPrice
    {
        public Guid TrackedPriceID { get; set; }
        public Guid UserID { get; set; }
        public Guid ProductID { get; set; }

        [Display(Name = "Desired Price")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal DesiredPrice { get; set; }

        [Display(Name = "Lowest Recorded Price")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal LowestPrice { get; set; }
        public string StoreWithLowestPrice { get; set; }
    }
}
