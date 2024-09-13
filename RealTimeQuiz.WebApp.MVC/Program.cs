using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Prometheus;
using RealTimeQuiz.Shared;
using RealTimeQuiz.WebApp.MVC.Hubs;

namespace RealTimeQuiz.WebApp.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSignalR();
            
            builder.Services.AddHttpClient(Constant.QuizParticipation, (serviceProvider, client) =>
            {                
                client.BaseAddress = new Uri("https://host.docker.internal:8031");
            }).ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            }); ;
            builder.Services.AddHttpClient(Constant.Leaderboard, (serviceProvider, client) =>
            {                
                client.BaseAddress = new Uri("https://host.docker.internal:8041");
            }).ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            }); ;
            builder.Services.AddHttpClient(Constant.ScoreUpdates, (serviceProvider, client) =>
            {   
                client.BaseAddress = new Uri("https://host.docker.internal:8021");
            }).ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.UseMetricServer();
            //adding metrics related to HTTP
            app.UseHttpMetrics(options =>
            {
                options.AddCustomLabel("host", context => context.Request.Host.Host);
            });


            app.MapHub<Messenger>("/messenger");
            app.Run();
        }
    }
}
