using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBot_V2.CommandModules
{
    public class BasicCommands : ModuleBase<SocketCommandContext>
    {        
        [Command("ping")]
        public async Task Pong()
        {
            await ReplyAsync($":ping_pong: Pong!: {Context.Client.Latency}ms");            
        }      

        [Command("pfp")]
        public async Task MyPfp()
        {
            await ReplyAsync(Context.User.GetAvatarUrl());
        }

        [Command("pfp")]
        public async Task MyPfp(IGuildUser _user)
        {
            await ReplyAsync(_user.GetAvatarUrl());
        }

        [Command("rng")]
        public async Task RNG(int min, int max)
        {            
            if (min < max)
            {
                int rng = new Random().Next(min, max);
                await ReplyAsync($":game_die: {rng}");
            }
            else if (min > max)
            {
                int rng = new Random().Next(max, min);
                await ReplyAsync($":game_die: {rng}");
            }
            else
            {
                await ReplyAsync($"What else should i choose dumbass");
            }
        }
    }
}
