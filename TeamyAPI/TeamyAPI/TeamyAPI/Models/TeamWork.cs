using System.Runtime.Serialization;

namespace TeamyAPI.Models
{
    [DataContract]
    public class TeamWork
    {
        [DataMember(Order = 0)]
        public string Name { get; set; }
        [DataMember(Order = 1)]
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
