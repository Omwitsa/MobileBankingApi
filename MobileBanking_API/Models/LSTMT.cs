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
    
    public partial class LSTMT
    {
        public long id { get; set; }
        public string cluster { get; set; }
        public string Class { get; set; }
        public string CDescription { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<long> YYear { get; set; }
        public Nullable<System.DateTime> transdate { get; set; }
        public Nullable<System.DateTime> transdate1 { get; set; }
    }
}
