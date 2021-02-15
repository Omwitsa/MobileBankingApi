using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileBanking_API.Controllers
{
    public class MemberModel
    {
        public string username { get; set; }

        public string password { get; set; }

        public string machineId { get; set; }
    }
}