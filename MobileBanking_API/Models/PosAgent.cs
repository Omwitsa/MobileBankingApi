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
    
    public partial class PosAgent
    {
        public long ID { get; set; }
        public string AgencyCode { get; set; }
        public string AgencyName { get; set; }
        public bool Active { get; set; }
        public string PhysicalAddress { get; set; }
        public string PINNo { get; set; }
        public string IDNo { get; set; }
        public string AccNo { get; set; }
        public string FloatAccNo { get; set; }
        public string CommissionAccNo { get; set; }
        public string PosSerialNo { get; set; }
        public string PosMobileNo { get; set; }
        public string PhoneNo { get; set; }
        public System.DateTime CreateOn { get; set; }
        public string CreatedBy { get; set; }
    }
}
