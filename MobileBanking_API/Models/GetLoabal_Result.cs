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
    
    public partial class GetLoabal_Result
    {
        public string LoanNo { get; set; }
        public string LoanCode { get; set; }
        public string MemberNo { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public Nullable<System.DateTime> FirstDate { get; set; }
        public Nullable<decimal> RepayRate { get; set; }
        public Nullable<System.DateTime> LastDate { get; set; }
        public Nullable<decimal> Interest { get; set; }
        public string RepayMethod { get; set; }
        public string Cleared { get; set; }
        public string AutoCalc { get; set; }
        public Nullable<decimal> IntrAmount { get; set; }
        public Nullable<int> RepayPeriod { get; set; }
        public string Remarks { get; set; }
        public string AuditID { get; set; }
        public Nullable<System.DateTime> AuditTime { get; set; }
        public Nullable<decimal> Loanbal { get; set; }
        public string Accno { get; set; }
        public long lid { get; set; }
        public Nullable<decimal> IntBalance { get; set; }
        public Nullable<decimal> Tamount { get; set; }
        public Nullable<System.DateTime> nextduedate { get; set; }
        public Nullable<decimal> Mintbal { get; set; }
        public Nullable<System.DateTime> duedate { get; set; }
        public Nullable<decimal> IntrOwed { get; set; }
        public Nullable<decimal> Penalty { get; set; }
        public Nullable<short> RepayMode { get; set; }
        public Nullable<System.DateTime> IntDate { get; set; }
        public string FQ { get; set; }
        public string TransactionNo { get; set; }
        public Nullable<double> LLF { get; set; }
        public bool Loaded { get; set; }
        public Nullable<int> Insurance { get; set; }
        public string GroupID { get; set; }
        public bool CRBLoan { get; set; }
    }
}
