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
    
    public partial class loantypeaging
    {
        public long id { get; set; }
        public string description { get; set; }
        public Nullable<int> nol { get; set; }
        public decimal amount { get; set; }
        public long @class { get; set; }
        public decimal Reqprov { get; set; }
        public Nullable<decimal> Pamount { get; set; }
        public Nullable<System.DateTime> Transdate { get; set; }
    }
}
