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
    
    public partial class FosaCharge
    {
        public string AccountCode { get; set; }
        public string Accno { get; set; }
        public decimal Amount { get; set; }
        public bool percentage { get; set; }
        public short RANGECODE { get; set; }
        public string Mode { get; set; }
        public string ChargeTrigger { get; set; }
    }
}
