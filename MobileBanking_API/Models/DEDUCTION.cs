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
    
    public partial class DEDUCTION
    {
        public long DeductionID { get; set; }
        public string CustNo { get; set; }
        public string AccNo { get; set; }
        public string MemberNo { get; set; }
        public string DedCode { get; set; }
        public Nullable<long> yYear { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> AmountInterest { get; set; }
        public Nullable<int> PartialPosted { get; set; }
        public bool Posted { get; set; }
        public Nullable<int> Period { get; set; }
        public Nullable<decimal> AmountCF { get; set; }
        public Nullable<decimal> AmountIntCF { get; set; }
        public string AuditID { get; set; }
        public Nullable<System.DateTime> AuditDateTime { get; set; }
        public string VoucherNo { get; set; }
        public Nullable<int> mMonth { get; set; }
        public Nullable<bool> UPD { get; set; }
        public Nullable<System.DateTime> TransDate { get; set; }
        public Nullable<System.DateTime> LastTransDate { get; set; }
        public Nullable<System.DateTime> MaturityDate { get; set; }
        public Nullable<int> Stopped { get; set; }
        public string WMNO { get; set; }
        public string ProductID { get; set; }
        public string RecoverFrom { get; set; }
        public string Source { get; set; }
        public Nullable<double> Arrears { get; set; }
        public Nullable<short> Processed { get; set; }
        public Nullable<System.DateTime> IntDate { get; set; }
        public bool CRBLoan { get; set; }
    }
}
