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
    
    public partial class vWBankTransaction
    {
        public string AccNO { get; set; }
        public string TransDescription { get; set; }
        public Nullable<System.DateTime> TransDate { get; set; }
        public string vno { get; set; }
        public string transType { get; set; }
        public Nullable<decimal> actualbalance { get; set; }
        public Nullable<int> yyear { get; set; }
        public string ChequeNo { get; set; }
        public Nullable<decimal> AvailableBalance { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string AccName { get; set; }
    }
}
