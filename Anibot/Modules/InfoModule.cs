using System;
using System.Collections.Generic;
using System.Text;
using Discord.Commands;
using Discord;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Anibot.Modules
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        [Command("info")]
        public async Task InfoAsync()
        {
            var timelapse = DateTime.UtcNow - Process.GetCurrentProcess().StartTime.ToUniversalTime();
            string timelapseString = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                                        timelapse.Hours,
                                        timelapse.Minutes,
                                        timelapse.Seconds,
                                        timelapse.Milliseconds);

            await ReplyAsync(embed: new EmbedBuilder()
            {
                Title = "Information",
                Description = $"Runtime: {timelapseString}",
                Color = new Color(0x77dd77)
            }.Build()) ;
        }
    }
}
