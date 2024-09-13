using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeQuiz.Shared.Models
{
    public class UpdateScoreResult
    {
        public bool Success { get; set; }
        public double NewScore { get; set; }
    }
}
