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
    
    public partial class EMPDEDUCTION
    {
        public long DedID { get; set; }
        public string PayrollNo { get; set; }
        public string DedNo { get; set; }
        public string DedName { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime FinishDate { get; set; }
        public decimal Amount { get; set; }
        public int Percentage { get; set; }
        public int Periodic { get; set; }
        public int Exempt { get; set; }
    }
}
