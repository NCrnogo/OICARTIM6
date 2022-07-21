using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Teamy.Models
{
    [DataContract(Name = "http://localhost:5000/api/Teams")]
    public class TeamWork
    {
        [Required]
        [DataMember(Order = 0)]
        public string Name { get; set; }
        [DataMember(Order = 1)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [DataMember(Order = 2)]
        public string Start { get; set; }
        [DataMember(Order = 3)]
        public string End { get; set; }
        [DataMember(Order = 4)]
        public string Date { get; set; }
        [DataMember(Order = 5)]
        public string TeamName { get; set; }
        [DataMember(Order = 6)]
        public string UserId { get; set; }
        [DataMember(Order = 7)]
        public string idWork { get; set; }
        [DataMember(Order = 8)]
        public string UserName { get; set; }
    }
}