//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebBanDTDD.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PostComment
    {
        public int CommentID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Detail { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> Rate { get; set; }
        public Nullable<int> PostID { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual Post Post { get; set; }
    }
}
