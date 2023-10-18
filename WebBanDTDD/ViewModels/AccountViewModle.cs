using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanDTDD.ViewModels
{
    public class AccountViewModle
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
    }
}