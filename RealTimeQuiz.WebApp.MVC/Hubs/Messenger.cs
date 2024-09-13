using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using RealTimeQuiz.Shared;
using RealTimeQuiz.Shared.Models;
using RealTimeQuiz.WebApp.MVC.Controllers;
using System.Xml;
using static System.Formats.Asn1.AsnWriter;

namespace RealTimeQuiz.WebApp.MVC.Hubs
{
    public class Messenger : Hub
    {

        private readonly ILogger<Messenger> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        public Messenger(IHttpClientFactory factory, ILogger<Messenger> logger)
        {
            _httpClientFactory = factory;
            _logger = logger;
        }
        public async Task SubmitAnswer(string quizId, string userId, string answer)
        {
            _logger.LogInformation($"User {userId} try to answer question");
            var client = _httpClientFactory.CreateClient(Constant.ScoreUpdates);
            var score = new Random().Next(1, 10); // random score regardless answer for tesing
            
            await client.PostAsJsonAsync($"Scores", new UpdateScoreRequest { QuizId = quizId, UserId = userId, Score = score });
            _logger.LogInformation($"User {userId} answered question");
            
            await BroadcastLeaderboardUpdate();
            
            
        }

        public async Task BroadcastLeaderboardUpdate()
        {
            _logger.LogInformation($"Broadcast Leaderboard Update");
            await Clients.All.SendAsync("LeaderboardUpdate", await GetLeaderboardModel());
        }

        public async Task GetLeaderboard()
        {
            _logger.LogInformation($"GetLeaderboard");            
            await Clients.All.SendAsync("LeaderboardUpdate", await GetLeaderboardModel());
        }

        private async Task<LeaderboardModel> GetLeaderboardModel()
        {
            var client = _httpClientFactory.CreateClient(Constant.Leaderboard);
            return await client.GetFromJsonAsync<LeaderboardModel>($"Leaderboard");
        }
    }
}
