using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanDTDD.Models;

namespace WebBanDTDD.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Product> NewArrivals { get; set; }
        public IEnumerable<Product> BestSellers { get; set; }
        public IEnumerable<Post> LatestBlogs { get; set; }
    }
}