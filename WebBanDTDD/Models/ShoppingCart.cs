using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanDTDD.Models;

namespace WebBanDTDD.Models
{
    public class ShoppingCart
    {
        Entities dbContext =new Entities();
        public int Id { get; set; } 
        public string Name { get; set; }
         public string Image { get; set; }
        public Double Price { get; set; }
        public int Quantity { get; set; }

        public Double PriceTotal {
            get
            {
                return Price * Quantity;
            }}

        public int ProductID { get; internal set; }

        public ShoppingCart(int id)
        {
            Id = id;
            Product product = dbContext.Products.Single(p => p.ProductID == id);
            Name = product.Name;
            Image = product.Image;
            Price = double.Parse(product.Price.ToString());
            Quantity = 1;


        }
    }
}