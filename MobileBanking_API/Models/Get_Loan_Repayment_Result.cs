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
    
    public partial class Get_Loan_Repayment_Result
    {
        public string LoanNo { get; set; }
        public string LoanCode { get; set; }
        public string MemberNo { get; set; }
        public Nullable<decimal> Principal { get; set; }
        public Nullable<decimal> Interest { get; set; }
        public Nullable<decimal> LoanBalance { get; set; }
        public Nullable<System.DateTime> DateReceived { get; set; }
        public string transby { get; set; }
    }
}
