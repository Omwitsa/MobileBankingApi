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
    
    public partial class PIN_Reset
    {
        public long ID { get; set; }
        public string IDNo { get; set; }
        public string PhoneNo { get; set; }
        public bool Reset { get; set; }
        public string AuditID { get; set; }
        public System.DateTime RequestDate { get; set; }
        public string FormSerialNo { get; set; }
        public string MachineID { get; set; }
        public string AuthorizedBy { get; set; }
        public string ReceivedBy { get; set; }
    }
}
