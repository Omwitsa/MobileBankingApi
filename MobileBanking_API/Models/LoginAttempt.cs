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
    
    public partial class LoginAttempt
    {
        public string Sessionid { get; set; }
        public string UserLoginId { get; set; }
        public System.DateTime time { get; set; }
        public string password { get; set; }
        public Nullable<bool> success { get; set; }
    }
}
