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
    
    public partial class BENEFIT
    {
        public long BenID { get; set; }
        public string PayrollNo { get; set; }
        public string BenNo { get; set; }
        public decimal MMonth { get; set; }
        public decimal YYear { get; set; }
        public decimal Amount { get; set; }
        public string BenDescription { get; set; }
        public int Taxable { get; set; }
    }
}
