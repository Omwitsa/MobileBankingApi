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
    
    public partial class Loan_Sector
    {
        public long ID { get; set; }
        public string SectorCode { get; set; }
        public string SectorName { get; set; }
        public string SubSectorCode { get; set; }
        public string SubSectorName { get; set; }
        public string AuditID { get; set; }
        public System.DateTime AuditTime { get; set; }
    }
}
