using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileBanking_API.Controllers
{
    public class LoginModel
    {
        public string FingerPrint { get; set; }
        public string IdNo { get; set; }

        public string MachineId { get; set; }
    }
}