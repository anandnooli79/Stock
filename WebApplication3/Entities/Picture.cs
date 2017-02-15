using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication3.Entities
{
    public class Picture
    {

        public int Id { get; set; }

        [Required]
        public string FilePath { get; set; }

        public string FileDescription { get; set; }

    }

    public class Stock
    {

        public int Id { get; set; }

        [Required]
        public string SKUId { get; set; }

        public string StockName { get; set; }

        public string StockDescription { get; set; }

        public string StockImage { get; set; }

        public double StockPrice { get; set; }

        public int DiscountPercent { get; set; }

        public bool IsInStock { get; set; }

    }
}