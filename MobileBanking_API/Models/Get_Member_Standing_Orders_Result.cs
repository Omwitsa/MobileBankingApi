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
    
    public partial class Get_Member_Standing_Orders_Result
    {
        public long ID { get; set; }
        public string MemberNo { get; set; }
        public string AccNo { get; set; }
        public string DeductionID { get; set; }
        public string STONo { get; set; }
        public decimal DedAmount { get; set; }
        public decimal Percentage { get; set; }
        public decimal DefAmount { get; set; }
        public string ProductID { get; set; }
        public string Source { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime LastTransDate { get; set; }
        public string AuditID { get; set; }
        public System.DateTime AuditTime { get; set; }
        public decimal Interest { get; set; }
        public decimal Principal { get; set; }
        public string AccNo2 { get; set; }
    }
}