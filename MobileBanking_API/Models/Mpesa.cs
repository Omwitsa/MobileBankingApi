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
    
    public partial class Mpesa
    {
        public long ID { get; set; }
        public System.DateTime TransDate { get; set; }
        public string AccNo { get; set; }
        public decimal Amount { get; set; }
        public bool Sent { get; set; }
        public string PhoneNo { get; set; }
        public string TransID { get; set; }
        public string orginconid { get; set; }
        public string Status { get; set; }
        public Nullable<int> ResponseCode { get; set; }
        public string MpesaName { get; set; }
        public string ChequeNo { get; set; }
    }
}
