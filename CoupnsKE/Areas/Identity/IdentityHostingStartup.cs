using System;
using CoupnsKE.Areas.Identity.Data;
using CoupnsKE.Data;
using kedzior.io.ConnectionStringConverter;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(CoupnsKE.Areas.Identity.IdentityHostingStartup))]
namespace CoupnsKE.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            string connectionString = Environment.GetEnvironmentVariable("MYSQLCONNSTR_localdb");
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<CoupnsKEContext>(options =>
                    options.UseMySql(
                        AzureMySQL.ToMySQLStandard(connectionString)));

                //services.AddDefaultIdentity<CoupnsKEUser>(options => options.SignIn.RequireConfirmedAccount = true)
                //    .AddEntityFrameworkStores<CoupnsKEContext>();
            });
        }
    }
}