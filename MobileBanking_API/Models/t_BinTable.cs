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
    
    public partial class t_BinTable
    {
        public Nullable<decimal> SearchOrder { get; set; }
        public string Description { get; set; }
        public string CardLow { get; set; }
        public string CardHigh { get; set; }
        public int ForwardToPort { get; set; }
        public string NewFormatID { get; set; }
        public string ForwardMessageProcedure { get; set; }
        public string VerifyPIN { get; set; }
    }
}
