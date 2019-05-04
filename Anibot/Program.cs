using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using Anibot.Services;

namespace Anibot
{
    class Program
    {
        public static void Main(string[] args)
        => new Program().MainAsync().GetAwaiter().GetResult();
        public async Task MainAsync()
        {
            var Client = new DiscordSocketClient();
            var Commande = new CommandService();
            var Handler = new CommandHandler(Client, Commande);
            Client.Log += Log;
            await Client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DiscordToken",
                EnvironmentVariableTarget.User));
            await Client.StartAsync();
            await Handler.InstallCommandsAsync();
            await Task.Delay(-1);
        }
        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg.ToString());
            return Task.CompletedTask;
        }
    }
}
