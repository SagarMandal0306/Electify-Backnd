using System.ComponentModel.DataAnnotations;

namespace VotingApp.Models
{
    public class RommsEntity
    {
        [Key]
        public int id { get; set; }
        public int roomid { get; set; }
        public int userid { get; set; }
        public string role { get; set; }
        public string status { get; set; }
    }
}
