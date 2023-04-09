using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace BidMyCar.Models
{
    public class CarDetailsDbContext : DbContext
    {
        
        public DbSet<CarDetail> CarDetails { get; set; }

        public CarDetailsDbContext() : base("name=BidMyCarEntities1")
        {
        }

    }
}