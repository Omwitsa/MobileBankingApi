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
    
    public partial class Acc_Updated_Result
    {
        public long customerbalanceid { get; set; }
        public string CustomerNo { get; set; }
        public string IDNo { get; set; }
        public string PayrollNo { get; set; }
        public string AccName { get; set; }
        public decimal Amount { get; set; }
        public Nullable<decimal> AvailableBalance { get; set; }
        public string AccNO { get; set; }
        public string TransDescription { get; set; }
        public Nullable<System.DateTime> TransDate { get; set; }
        public Nullable<decimal> Commission { get; set; }
        public string ChequeNo { get; set; }
        public string Period { get; set; }
        public Nullable<bool> Posted { get; set; }
        public Nullable<bool> Locked { get; set; }
        public string transType { get; set; }
        public Nullable<bool> Status { get; set; }
        public string vno { get; set; }
        public string Auditid { get; set; }
        public Nullable<System.DateTime> auditdate { get; set; }
        public string moduleid { get; set; }
        public string accd { get; set; }
        public Nullable<System.DateTime> valuedate { get; set; }
        public Nullable<decimal> actualbalance { get; set; }
        public Nullable<bool> Cash { get; set; }
        public string bcode { get; set; }
        public Nullable<bool> bosa { get; set; }
        public Nullable<bool> rebuild { get; set; }
        public Nullable<short> Reconciled { get; set; }
        public System.DateTime ReconDate { get; set; }
        public string AuthorizedBy { get; set; }
        public string AccMain { get; set; }
        public string MachineID { get; set; }
        public string TransactionNo { get; set; }
        public string pmode { get; set; }
        public string transcode { get; set; }
    }
}
