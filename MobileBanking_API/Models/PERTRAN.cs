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
    
    public partial class PERTRAN
    {
        public string MemberNo { get; set; }
        public Nullable<System.DateTime> TransDate { get; set; }
        public Nullable<decimal> OpenShares { get; set; }
        public Nullable<decimal> CloseShares { get; set; }
        public Nullable<decimal> OpenLoanBal { get; set; }
        public Nullable<decimal> CloseLoanBal { get; set; }
        public Nullable<decimal> IntrPaid { get; set; }
        public Nullable<decimal> IntrCharged { get; set; }
        public Nullable<decimal> NewLoans { get; set; }
        public Nullable<decimal> IntrOwed { get; set; }
        public decimal ShareCap { get; set; }
    }
}
