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
    
    public partial class IMPORT
    {
        public long id { get; set; }
        public string MemberNo { get; set; }
        public string Accno { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> principal { get; set; }
        public Nullable<decimal> interest { get; set; }
        public Nullable<long> post { get; set; }
        public string auditid { get; set; }
        public Nullable<System.DateTime> auditdatetime { get; set; }
    }
}
