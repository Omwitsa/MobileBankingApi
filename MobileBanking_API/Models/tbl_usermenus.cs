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
    
    public partial class tbl_usermenus
    {
        public long id { get; set; }
        public string groupname { get; set; }
        public string menu { get; set; }
        public Nullable<System.DateTime> regdate { get; set; }
        public bool enable { get; set; }
        public string userloginid { get; set; }
    }
}
