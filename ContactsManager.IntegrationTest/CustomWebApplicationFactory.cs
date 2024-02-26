using Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        //INTHIS WE HAVE To remove the addbdcontext in program .cs and use inmemory entity framework.
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.UseEnvironment("Test");

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(temp => temp.ServiceType == typeof(DbContextOptions<ApplicationDBContext>));

                if(descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<ApplicationDBContext>(options =>
                {
                    options.UseInMemoryDatabase("DatabaseForTesting");
                });
            });
        }
    }
}
