using System.ComponentModel.DataAnnotations;

namespace VotingApp.Models
{
    public class OptionsEntity
    {
        [Key]
        public int optionid { get; set; }
        public int pollid { get; set; }
        public string option { get; set; }
    }
}
