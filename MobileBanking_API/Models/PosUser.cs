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
    
    public partial class PosUser
    {
        public long ID { get; set; }
        public string IDNo { get; set; }
        public string Name { get; set; }
        public string AgencyCode { get; set; }
        public string PhoneNo { get; set; }
        public bool Active { get; set; }
        public string FingerPrint1 { get; set; }
        public string FingerPrint2 { get; set; }
        public string PosSerialNo { get; set; }
        public bool Admin { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public bool Teller { get; set; }
    }
}
