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
    
    public partial class Client
    {
        public long ID { get; set; }
        public string Surname { get; set; }
        public string OtherName { get; set; }
        public string PhoneNo { get; set; }
        public string PIN { get; set; }
        public string PIN_Status { get; set; }
        public string IDNo { get; set; }
        public bool Locked { get; set; }
        public bool Unsubscribed { get; set; }
        public string alternativePhoneNo { get; set; }
        public bool Authorized { get; set; }
    }
}
