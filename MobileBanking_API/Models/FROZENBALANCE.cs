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
    
    public partial class FROZENBALANCE
    {
        public long ID { get; set; }
        public string AccNo { get; set; }
        public System.DateTime TransDate { get; set; }
        public string TransDescription { get; set; }
        public double Amount { get; set; }
        public string TransType { get; set; }
        public string AuditID { get; set; }
        public System.DateTime AuditTime { get; set; }
    }
}
