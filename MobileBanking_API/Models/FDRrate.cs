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
    
    public partial class FDRrate
    {
        public long code { get; set; }
        public Nullable<decimal> oamount { get; set; }
        public Nullable<decimal> uamount { get; set; }
        public string rate { get; set; }
        public Nullable<int> period { get; set; }
    }
}
