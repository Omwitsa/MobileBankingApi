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
    
    public partial class Get_Teller_Transactions_Result
    {
        public long ID { get; set; }
        public System.DateTime TransDate { get; set; }
        public decimal Amount { get; set; }
        public string DrAccNo { get; set; }
        public string CrAccNo { get; set; }
        public string DocumentNo { get; set; }
        public string Source { get; set; }
        public string TransDescript { get; set; }
        public Nullable<System.DateTime> AuditTime { get; set; }
        public string AuditID { get; set; }
        public string ChequeNo { get; set; }
        public Nullable<bool> Recon { get; set; }
        public string TransactionNo { get; set; }
        public string Module { get; set; }
        public Nullable<int> ReconId { get; set; }
        public string PMode { get; set; }
        public string RefId { get; set; }
        public string LoanNo { get; set; }
        public Nullable<decimal> agentCommission { get; set; }
        public Nullable<decimal> saccoCommission { get; set; }
    }
}
