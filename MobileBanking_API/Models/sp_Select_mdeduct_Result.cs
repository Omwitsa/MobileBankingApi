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
    
    public partial class sp_Select_mdeduct_Result
    {
        public string MemberNo { get; set; }
        public string RefNo { get; set; }
        public string Description { get; set; }
        public Nullable<int> TransCode { get; set; }
        public Nullable<decimal> Principal { get; set; }
        public Nullable<decimal> Interest { get; set; }
        public Nullable<decimal> SHARES { get; set; }
        public Nullable<decimal> Total { get; set; }
        public int Mmonth { get; set; }
        public int yyear { get; set; }
        public Nullable<decimal> Amt_Received { get; set; }
        public Nullable<bool> posted { get; set; }
        public Nullable<decimal> entrancefee { get; set; }
        public Nullable<decimal> passbook { get; set; }
        public Nullable<decimal> memberid { get; set; }
        public Nullable<decimal> othercharges { get; set; }
        public Nullable<decimal> loanform { get; set; }
        public Nullable<decimal> calender { get; set; }
        public string remarks { get; set; }
        public Nullable<bool> postedtogl { get; set; }
        public long MdeductID { get; set; }
    }
}
