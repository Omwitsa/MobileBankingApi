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
    
    public partial class Stmt
    {
        public string AdvNo { get; set; }
        public string AccNo { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> AmountInterest { get; set; }
        public Nullable<decimal> AmountRepaid { get; set; }
        public string TransDescription { get; set; }
        public Nullable<System.DateTime> TransDate { get; set; }
        public string TransType { get; set; }
        public string DedCode { get; set; }
        public Nullable<bool> IsAdvance { get; set; }
        public long ID { get; set; }
    }
}
