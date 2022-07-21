using System.Runtime.Serialization;

namespace TeamyAPI.Models
{
    [DataContract]
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
