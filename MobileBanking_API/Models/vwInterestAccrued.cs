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
    
    public partial class vwInterestAccrued
    {
        public string MemberNo { get; set; }
        public string Surname { get; set; }
        public string OtherNames { get; set; }
        public Nullable<decimal> InterestAccrued { get; set; }
        public Nullable<int> mmonth { get; set; }
        public string loanno { get; set; }
        public Nullable<int> yyear { get; set; }
    }
}