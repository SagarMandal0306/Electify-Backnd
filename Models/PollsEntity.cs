using System.ComponentModel.DataAnnotations;

namespace VotingApp.Models
{
    public class PollsEntity
    {
        [Key]
        public int pollid { get; set; }
        public string title { get; set; }
        public string desc { get; set; }
        public int roomid { get; set; }
        public string createat { get; set; }
        public string endat { get; set; }
    }
}
