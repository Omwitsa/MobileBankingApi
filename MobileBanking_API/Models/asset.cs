//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MobileBanking_API.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class asset
    {
        public int assetId { get; set; }
        public string assetNo { get; set; }
        public string assetName { get; set; }
        public Nullable<int> assetCode { get; set; }
        public Nullable<int> DescripId { get; set; }
        public Nullable<System.DateTime> purchaseDate { get; set; }
        public Nullable<decimal> purchasePrice { get; set; }
        public string vendor { get; set; }
        public string location { get; set; }
        public string serialNo { get; set; }
        public Nullable<int> unitNo { get; set; }
        public Nullable<decimal> currentValue { get; set; }
        public Nullable<decimal> salvageValue { get; set; }
        public Nullable<int> usefulLife { get; set; }
        public Nullable<int> months { get; set; }
        public Nullable<int> departCode { get; set; }
        public Nullable<int> status { get; set; }
        public System.DateTime auditDate { get; set; }
        public string auditId { get; set; }
        public string mcode { get; set; }
        public Nullable<int> allocTracker { get; set; }
        public string assetType { get; set; }
        public string DRAccNo { get; set; }
        public string CRAccNo { get; set; }
        public Nullable<decimal> DepRate { get; set; }
        public decimal RevalueAmount { get; set; }
        public System.DateTime RevalueDate { get; set; }
        public string RevalueBy { get; set; }
        public string BranchName { get; set; }
    }
}
