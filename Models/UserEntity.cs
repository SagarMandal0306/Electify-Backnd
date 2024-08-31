using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace VotingApp.Models
{
    public class UserEntity
    {
        [Key]
        public int userid { get; set; }
        public string username { get; set; }
        public string useremail { get; set; }
        [JsonIgnore]
        public string password { get; set; }
        public string role { get; set; }
    }
}
