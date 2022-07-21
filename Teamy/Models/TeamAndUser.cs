using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Teamy.Models
{
    [DataContract(Name = "http://localhost:5000/api/Teams")]
    public class TeamAndUser
    {
        [DataMember(Order = 0)]
        public int IdUser { get; set; }
        [DataMember(Order = 1)]
        public string TeamName { get; set; }
    }
}