using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CouponsKE.Models
{
    public class UserCoupons
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public Guid UserID { get; set; }
        public List<Coupon> Coupons { get; set; }

    }
}
