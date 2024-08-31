namespace VotingApp.DTO
{
    public class PollsUpdateDto
    {
        public int pollid { get; set; }
        public string title { get; set; }
        public string desc { get; set; }
        public int durationInMinutes { get; set; }
        public int durationInHours { get; set; }
    }
}
