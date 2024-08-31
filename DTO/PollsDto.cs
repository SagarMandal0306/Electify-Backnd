namespace VotingApp.DTO
{
    public class PollsDto
    {
        public string title { get; set; }
        public string desc { get; set; }
        public int roomid { get; set; }
        public int durationInMinutes { get; set; }
        public int durationInHours { get; set; }
    }
}
