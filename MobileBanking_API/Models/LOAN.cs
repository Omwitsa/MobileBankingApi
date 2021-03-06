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
    
    public partial class LOAN
    {
        public long LoanID { get; set; }
        public string LoanNo { get; set; }
        public string MemberNo { get; set; }
        public string LoanCode { get; set; }
        public Nullable<System.DateTime> ApplicDate { get; set; }
        public Nullable<decimal> LoanAmt { get; set; }
        public Nullable<int> RepayPeriod { get; set; }
        public string JobGrp { get; set; }
        public Nullable<decimal> BasicSalary { get; set; }
        public string WitMemberNo { get; set; }
        public string WitSigned { get; set; }
        public string SupMemberNo { get; set; }
        public string SupSigned { get; set; }
        public string PreparedBy { get; set; }
        public string Purpose { get; set; }
        public string AddSecurity { get; set; }
        public Nullable<decimal> Insurance { get; set; }
        public Nullable<decimal> InsPercent { get; set; }
        public Nullable<int> InsCalcType { get; set; }
        public string Posted { get; set; }
        public string AuditID { get; set; }
        public Nullable<System.DateTime> AuditTime { get; set; }
        public Nullable<decimal> Interest { get; set; }
        public string repaymethod { get; set; }
        public string transactionNo { get; set; }
        public string mode { get; set; }
        public string PMode { get; set; }
        public Nullable<int> status { get; set; }
        public string PurposeCode { get; set; }
    }
}
