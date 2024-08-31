using System.ComponentModel.DataAnnotations;

namespace VotingApp.Models
{
    public class VoteEntity
    {
        [Key]
        public int voteid { get; set; }
        public int userid { get; set; }
        public int optionid { get; set; }
        public int pollid { get; set; }
        public DateTime votedat { get; set; }
    }
}
