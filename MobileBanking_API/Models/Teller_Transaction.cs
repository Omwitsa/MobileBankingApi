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
    
    public partial class Teller_Transaction
    {
        public long tellerid { get; set; }
        public string TellerName { get; set; }
        public string CubicleNumber { get; set; }
        public Nullable<decimal> Deposits { get; set; }
        public Nullable<decimal> Withdrawals { get; set; }
        public string AccountNumber { get; set; }
        public string tgno { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public string vno { get; set; }
        public Nullable<bool> Posted { get; set; }
        public Nullable<bool> Locked { get; set; }
        public string AuditId { get; set; }
        public Nullable<System.DateTime> AuditTime { get; set; }
        public string Transdescription { get; set; }
        public Nullable<System.DateTime> CHQDate { get; set; }
        public Nullable<System.DateTime> ccdate { get; set; }
        public string Chequeno { get; set; }
        public Nullable<decimal> CHQAMOUNT { get; set; }
        public Nullable<decimal> balance { get; set; }
        public string PAYNO { get; set; }
        public string IDNO { get; set; }
        public string NAME { get; set; }
        public string ACCNAME { get; set; }
        public Nullable<decimal> Commission { get; set; }
        public Nullable<bool> printed { get; set; }
        public Nullable<bool> CASH { get; set; }
    }
}
