using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CouponsKE.Models;

namespace CoupnsKE.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<CouponsKE.Models.Coupon> Coupon { get; set; }
        public DbSet<CouponsKE.Models.Deals> Deals { get; set; }
        public DbSet<CouponsKE.Models.Product> Product { get; set; }
        public DbSet<CouponsKE.Models.Store> Store { get; set; }
        public DbSet<CouponsKE.Models.TrackedPrice> TrackedPrice { get; set; }
        public DbSet<CouponsKE.Models.UserCoupons> UserCoupons { get; set; }
    }
}
