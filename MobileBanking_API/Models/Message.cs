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
    
    public partial class Message
    {
        public long Id { get; set; }
        public string Telephone { get; set; }
        public string Content { get; set; }
        public string ProcessTime { get; set; }
        public string MsgType { get; set; }
        public Nullable<bool> Charged { get; set; }
        public string AlertType { get; set; }
        public Nullable<System.DateTime> DateReceived { get; set; }
        public string Source { get; set; }
        public string AccNo { get; set; }
        public string Code { get; set; }
        public string LoanNo { get; set; }
        public bool Processed { get; set; }
    }
}
