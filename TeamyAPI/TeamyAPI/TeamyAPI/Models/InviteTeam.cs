using System.Runtime.Serialization;

namespace TeamyAPI.Models
{
    [DataContract]
    public class InviteTeam
    {
        [DataMember(Order = 0)]
        public string UserName { get; set; }
        [DataMember(Order = 1)]
        public string TeamId { get; set; }
    }
}
