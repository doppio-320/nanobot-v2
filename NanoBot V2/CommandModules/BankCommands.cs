using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBot_V2.CommandModules
{
    public class BankCommands : ModuleBase<SocketCommandContext>
    {
        [Command("balance")]
        [Alias("bal")]
        [RequireContext(ContextType.Guild)]
        public async Task Balance()
        {
            await ReplyAsync($"{Context.User.Username}'s balance: {Services.GuildServices.GetGuildData(Context.Guild.Id).GetUserData(Context.User.Id).balance.balance} credits.");
        }
    }
}
