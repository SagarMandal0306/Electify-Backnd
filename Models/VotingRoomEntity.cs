using System.ComponentModel.DataAnnotations;

namespace VotingApp.Models
{
    public class VotingRoomEntity
    {
        [Key]
        public int id { get; set; }
        public int roomid { get; set; }
        public string title { get; set; }
        public string desc { get; set; }
        public int userid { get; set; }
        public DateTime createdat { get; set; }

    }
}
