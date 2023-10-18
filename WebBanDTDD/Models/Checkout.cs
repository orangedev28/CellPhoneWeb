using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanDTDD.Models
{
    public class Checkout
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string DeliveryAddress { get; set; }
        [Required]
        public int Ordertype { get; set; }


    }
}