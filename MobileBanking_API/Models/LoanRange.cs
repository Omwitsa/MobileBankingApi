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
    
    public partial class LoanRange
    {
        public int Loanid { get; set; }
        public Nullable<int> LoanRangecode { get; set; }
        public Nullable<decimal> Loanfrom { get; set; }
        public Nullable<decimal> loanto { get; set; }
        public Nullable<decimal> Amount { get; set; }
    }
}
