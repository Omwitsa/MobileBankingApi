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
    
    public partial class Get_summaryCollectionDistribution_Result
    {
        public string groupid { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> contribution { get; set; }
        public Nullable<decimal> Interest { get; set; }
        public string MemberNo { get; set; }
        public Nullable<decimal> Principal { get; set; }
        public string GroupName { get; set; }
        public string Surname { get; set; }
        public string OtherNames { get; set; }
    }
}
