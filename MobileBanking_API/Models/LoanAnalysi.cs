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
    
    public partial class LoanAnalysi
    {
        public long ID { get; set; }
        public string MemberNo { get; set; }
        public string LoanNo { get; set; }
        public System.DateTime LoanDate { get; set; }
        public decimal Expected { get; set; }
        public decimal Guarantor { get; set; }
    }
}
