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
    
    public partial class LoanGuarantor
    {
        public long ID { get; set; }
        public string LoanNo { get; set; }
        public string Guarantor { get; set; }
        public string Loanee { get; set; }
        public decimal Amount { get; set; }
        public string gAccNo { get; set; }
        public string lAccNo { get; set; }
        public bool posted { get; set; }
    }
}
