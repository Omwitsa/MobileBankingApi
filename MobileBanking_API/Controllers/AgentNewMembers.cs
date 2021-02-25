using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileBanking_API.Controllers
{
    public class AgentNewMembers
    {


        public string Surname { get; set; }
        public string other_Names { get; set; }
        public string idno { get; set; }
        public string DOB { get; set; }
        public string mobile_number { get; set; }
        public string Gender { get; set; }
        public string FingerPrint1 { get; set; }
        public string FingerPrint2 { get; set; }
        public string MachineId { get; set; }
        public string Agentid { get; set; }
    }
}