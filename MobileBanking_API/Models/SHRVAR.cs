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
    
    public partial class SHRVAR
    {
        public string MemberNo { get; set; }
        public Nullable<decimal> OldContr { get; set; }
        public Nullable<decimal> NewContr { get; set; }
        public Nullable<System.DateTime> VarDate { get; set; }
        public string AuditID { get; set; }
        public Nullable<System.DateTime> AuditTime { get; set; }
        public string SharesCode { get; set; }
        public Nullable<int> subscribed { get; set; }
    }
}
