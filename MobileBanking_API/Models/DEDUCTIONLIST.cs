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
    
    public partial class DEDUCTIONLIST
    {
        public long id { get; set; }
        public string DedCode { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public Nullable<int> Chargeintupfront { get; set; }
        public Nullable<double> InterestRate { get; set; }
        public Nullable<bool> CalcInterest { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string GlAccount { get; set; }
        public Nullable<int> OrderNo { get; set; }
        public Nullable<int> UseRange { get; set; }
        public string GLControl { get; set; }
        public string Glaccrued { get; set; }
        public decimal Period { get; set; }
        public string Recoverfrom { get; set; }
        public string source { get; set; }
        public bool Mobile { get; set; }
    }
}
