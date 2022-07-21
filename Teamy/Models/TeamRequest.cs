using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Teamy.Models
{
    [DataContract(Name = "http://localhost:5000/api/Teams")]
    public class TeamRequest
    {
        [DataMember(Order = 0)]
        public string UserName { get; set; }
        [DataMember(Order = 2)]
        public string UserId { get; set; }
        [DataMember(Order = 3)]
        public string TeamId { get; set; }
    }
}