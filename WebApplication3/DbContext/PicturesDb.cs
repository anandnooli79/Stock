using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApplication3.Entities;

namespace WebApplication3
{
    public class PicturesDb: DbContext
    {   
        public PicturesDb():base("DefaultConnection")
        {

        }

        public DbSet<Picture> Pictures
        {
            get;
            set;
        }

        public DbSet<Stock> AvailableStock
        {
            get;
            set;
        }
    }
}