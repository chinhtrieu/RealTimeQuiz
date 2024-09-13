using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Shared
{
    public class APISettings
    {
        public const string Name = "APISettings";
        public string LeaderboardApiUrl { get; set; }
        public string QuizParticipationApiUrl { get; set; }
        public string ScoreUpdatesApiUrl { get; set; }
    }
}
