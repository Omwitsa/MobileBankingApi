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
    
    public partial class loanagingdetailed
    {
        public long lid { get; set; }
        public string member { get; set; }
        public string accno { get; set; }
        public string loanno { get; set; }
        public Nullable<System.DateTime> dateissued { get; set; }
        public string age { get; set; }
        public string descr { get; set; }
        public Nullable<decimal> loanamnt { get; set; }
        public Nullable<decimal> balance { get; set; }
    }
}
