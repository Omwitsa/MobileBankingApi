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
    
    public partial class Check_Advert_Result
    {
        public long id { get; set; }
        public string Advertisement { get; set; }
        public Nullable<bool> status { get; set; }
        public string accno { get; set; }
        public Nullable<decimal> amount { get; set; }
        public string username { get; set; }
        public string vno { get; set; }
        public int Authorized { get; set; }
        public string TransType { get; set; }
        public string Reason { get; set; }
        public string AuthorizedBy { get; set; }
        public System.DateTime AuthorizedDate { get; set; }
        public Nullable<System.DateTime> RequestDate { get; set; }
    }
}
