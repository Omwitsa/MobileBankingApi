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
    
    public partial class CASHINCOME
    {
        public long ID { get; set; }
        public string MemberNo { get; set; }
        public Nullable<System.DateTime> TransDate { get; set; }
        public Nullable<System.DateTime> period { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string AuditID { get; set; }
        public System.DateTime AuditTime { get; set; }
        public int Posted { get; set; }
        public string Source { get; set; }
        public string Center { get; set; }
        public string Chequeno { get; set; }
        public string Productid { get; set; }
    }
}
