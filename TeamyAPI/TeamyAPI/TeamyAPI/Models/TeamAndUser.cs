﻿using System.Runtime.Serialization;

namespace TeamyAPI.Models
{
    [DataContract]
    public class TeamAndUser
    {
        [DataMember(Order = 0)]
        public int IdUser { get; set; }
        [DataMember(Order = 1)]
        public string TeamName { get; set; }
    }
}
