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
    
    public partial class BRIDGINGLOAN
    {
        public string LoanNo { get; set; }
        public System.DateTime ApplicDate { get; set; }
        public string BrgLoanNo { get; set; }
        public decimal Balance { get; set; }
        public string AuditID { get; set; }
        public System.DateTime AuditTime { get; set; }
        public bool Paid { get; set; }
        public short BridgeType { get; set; }
    }
}
