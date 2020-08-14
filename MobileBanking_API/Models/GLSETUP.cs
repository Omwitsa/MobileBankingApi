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
    
    public partial class GLSETUP
    {
        public long glid { get; set; }
        public string GlCode { get; set; }
        public string GlAccName { get; set; }
        public string AccNo { get; set; }
        public string GlAccType { get; set; }
        public string GlAccGroup { get; set; }
        public string NormalBal { get; set; }
        public string GlAccStatus { get; set; }
        public decimal Bal { get; set; }
        public decimal Curr_Code { get; set; }
        public string AuditOrg { get; set; }
        public string AuditID { get; set; }
        public System.DateTime AuditDate { get; set; }
        public string Curr { get; set; }
        public decimal Actuals { get; set; }
        public decimal Budgetted { get; set; }
        public System.DateTime TransDate { get; set; }
        public bool IsSubLedger { get; set; }
        public string AccCategory { get; set; }
        public string GLAccMain { get; set; }
        public decimal OpeningBal { get; set; }
        public string SubAcc { get; set; }
        public string SubLedgerAcc { get; set; }
        public long Bank { get; set; }
        public string SubGroup { get; set; }
        public Nullable<long> Cashflow { get; set; }
        public Nullable<long> Capital { get; set; }
        public Nullable<long> Liquidity { get; set; }
        public Nullable<long> IR { get; set; }
        public Nullable<decimal> Obal { get; set; }
        public Nullable<System.DateTime> Obaldate { get; set; }
        public Nullable<long> Class { get; set; }
        public string Fcluster { get; set; }
        public string Scluster { get; set; }
        public string Effects { get; set; }
        public string Tcluster { get; set; }
        public string Focluster { get; set; }
        public string Cflow { get; set; }
        public Nullable<long> Balsht { get; set; }
        public string Cbal { get; set; }
        public Nullable<long> CIn { get; set; }
        public string CInc { get; set; }
        public string Cflowdesc { get; set; }
        public Nullable<System.DateTime> newglopeningbaldate { get; set; }
        public string Type { get; set; }
        public Nullable<double> currentbal { get; set; }
        public Nullable<double> newglopeningbal { get; set; }
        public string NarrationID { get; set; }
        public Nullable<int> isREarning { get; set; }
        public string GLAccMainGroup { get; set; }
        public Nullable<bool> isSuspense { get; set; }
        public string SubType { get; set; }
        public string AccGroup { get; set; }
        public string tenants { get; set; }
        public double sto { get; set; }
    }
}
