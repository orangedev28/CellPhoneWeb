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
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.InvoiceDetails = new HashSet<InvoiceDetail>();
            this.OrderDetails = new HashSet<OrderDetail>();
            this.ProductComments = new HashSet<ProductComment>();
        }
    
        public int ProductID { get; set; }
        public string Name { get; set; }
        public Nullable<bool> Status { get; set; }
        public string Image { get; set; }
        public string Listimage { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> PromotionPrice { get; set; }
        public Nullable<bool> VAT { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> Warranty { get; set; }
        public Nullable<System.DateTime> Hot { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public Nullable<int> CateID { get; set; }
        public Nullable<int> BrandID { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public string Tags { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> ViewCount { get; set; }
        public Nullable<int> Capacity { get; set; }
        public string Color { get; set; }
    
        public virtual Brand Brand { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual Supplier Supplier { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductComment> ProductComments { get; set; }
    }
}
