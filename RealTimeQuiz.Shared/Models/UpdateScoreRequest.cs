using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Shared.Models
{
    public class UpdateScoreRequest
    {
        public string QuizId { get; set; }
        public string UserId { get; set; }
        public int Score { get; set; }
    }
}
