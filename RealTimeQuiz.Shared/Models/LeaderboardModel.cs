namespace RealTimeQuiz.Shared.Models
{
    public class LeaderboardModel
    {
        public List<LeaderboardMember> Members { get; set; }


    }
    public class LeaderboardMember
    {
        public string UserId { get; set; }
        public int Score { get; set; }
    }

}
