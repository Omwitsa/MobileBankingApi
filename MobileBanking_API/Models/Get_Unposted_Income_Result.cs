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
    
    public partial class Get_Unposted_Income_Result
    {
        public long ID { get; set; }
        public string MemberNo { get; set; }
        public string AccNo { get; set; }
        public string IDNo { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public System.DateTime TransDate { get; set; }
        public Nullable<System.DateTime> Period { get; set; }
        public string Source { get; set; }
        public Nullable<int> Posted { get; set; }
        public string ProductID { get; set; }
        public string AuditID { get; set; }
        public string AuditTime { get; set; }
        public string ChequeNo { get; set; }
        public decimal NetIncome { get; set; }
        public bool Processed { get; set; }
        public decimal PostingCharge { get; set; }
        public string Name { get; set; }
    }
}
