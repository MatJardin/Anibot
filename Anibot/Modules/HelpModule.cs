using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Discord.Addons.Interactive;
using System.Linq;
using MoreLinq;
using SatiriquesBot.Attributes;

namespace Anibot.Modules
{
    [Group("help")]
    [Alias("h")]
    [ExcludeFromHelp]
    public class HelpModule : InteractiveBase<SocketCommandContext>
    {
        private CommandService _commandService = new CommandService();
        public HelpModule(CommandService service)
        {
            _commandService = service;
        }

        [Command(RunMode = RunMode.Async)]
        [Name("Help")]
        public async Task HelpAsync()
        {
            var commands = _commandService.Commands
                .Where(x=>!x.Module.Attributes.Any(z=>z.GetType() == typeof(ExcludeFromHelpAttribute)))
                .Where(x=>!x.Attributes.Any(z=>z.GetType() == typeof(ExcludeFromHelpAttribute)))
                .DistinctBy(x => x.Aliases[0])
                .GroupBy(x => x.Module)
                .OrderBy(x=>x.Key.Name)
                .ToDictionary(x => x.Key);

            await PagedReplyAsync(new PaginatedMessage()
            {
                Pages = commands.Select(x => HelpPageBuilder
                                .Build(x.Value.OrderBy(y=>y.Name), x.Key))
            }, new ReactionList() { First = true, Last = true, Trash = true });
        }
        
        [Command]
        public async Task HelpAsync(string query)
        {
            var commands = _commandService.Commands
                .Where(x => x.Aliases.Any(y => y == query.ToLower()))
                .ToArray();

            if (commands.Any())
            {
                var signatures = commands.Select(x =>
                $"'{Format.Bold(x.Aliases[0])} " +
                $"{string.Join(" ", x.Parameters.Select(y => (y.IsOptional ? $"[{y.Name}]" : y.Name)))}" +
                $"{(string.IsNullOrWhiteSpace(x.Summary) ? "" : Environment.NewLine + x.Summary + Environment.NewLine)}");

                await ReplyAsync(embed: new EmbedBuilder()
                {
                    Title = $"Module {commands[0].Module.Name}, Command {commands[0].Name}",
                    Description = string.Join(Environment.NewLine, signatures),
                    Footer = new EmbedFooterBuilder()
                    {
                        Text = (commands[0].Aliases.Count > 1 ? $"Aliases: {string.Join(", ",commands[0].Aliases.Skip(1))}" : ""),
                    }
                }.Build());
            }
                
        }
    }
}