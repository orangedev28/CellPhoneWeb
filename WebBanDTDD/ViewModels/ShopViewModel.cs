using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanDTDD.Models;

namespace WebBanDTDD.ViewModels
{
    public class ShopViewModel
    {
        public IEnumerable<Product> ListPhone { get; set; }
        public IEnumerable<Product> ListTabLate { get; set; }
        public IEnumerable<Product> ListAccessory { get; set; }

    }
}