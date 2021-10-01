using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;
using Discord;
using System.IO;
using NanoBot_V2.Services;

namespace NanoBot_V2.CommandModules
{
    public class GuildCommands : ModuleBase<SocketCommandContext>
    {
        #region Config
        [Command("configureChatChannel")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task ConfigChatChannel(IGuildChannel _channel = null)
        {
            if (_channel == null)
            {
                GuildServices.GetGuildData(Context.Guild.Id).chatChannelID = 0;
                await Context.Message.AddReactionAsync(new Emoji("🗑"));
            }
            else
            {
                GuildServices.GetGuildData(Context.Guild.Id).chatChannelID = _channel.Id;
                await Context.Message.AddReactionAsync(new Emoji("✅"));
            }
        }

        [Command("configureLogChannel")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task ConfigLogChannel(IGuildChannel _channel = null)
        {
            if (_channel == null)
            {
                GuildServices.GetGuildData(Context.Guild.Id).logChannelID = 0;
                await Context.Message.AddReactionAsync(new Emoji("🗑"));
            }
            else
            {
                GuildServices.GetGuildData(Context.Guild.Id).logChannelID = _channel.Id;
                LoggingServices.LogChannelSet(Context);
                await Context.Message.AddReactionAsync(new Emoji("✅"));
            }
        }

        [Command("configureMuteRole")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task ConfigMuteRole(IRole _role = null)
        {
            if (_role == null)
            {
                GuildServices.GetGuildData(Context.Guild.Id).muteRoleID = 0;
                await Context.Message.AddReactionAsync(new Emoji("🗑"));
            }
            else
            {
                GuildServices.GetGuildData(Context.Guild.Id).muteRoleID = _role.Id;
                await Context.Message.AddReactionAsync(new Emoji("✅"));
            }
        }

        [Command("supressRankUpNotif")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task ConfigRankUpMute()
        {
            GuildServices.GetGuildData(Context.Guild.Id).supressRankUpMessage = !GuildServices.GetGuildData(Context.Guild.Id).supressRankUpMessage;

            if (GuildServices.GetGuildData(Context.Guild.Id).supressRankUpMessage)
            {
                await ReplyAsync("Muted rank up notifications.");
            }
            else
            {
                await ReplyAsync("Resumed rank up notifications.");
            }
            await Context.Message.AddReactionAsync(new Emoji("✅"));
        }

        #endregion

        [Command("card")]
        [RequireContext(ContextType.Guild)]
        public async Task DisplayUserCard()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                //var userData = GuildServices.GetGuildData(Context.Guild.Id).GetUserData(Context.User.Id);
                var bitmap = GraphicsGenerator.GenerateUserCard(Context.Guild.Id, Context.User.Id);
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                memoryStream.Seek(0, SeekOrigin.Begin);
                bitmap.Dispose();

                await Context.Channel.SendFileAsync(memoryStream, "generated.png");
            }
        }

        [Command("card")]
        [RequireContext(ContextType.Guild)]
        public async Task DisplayUserCard(IGuildUser _user)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                //var userData = GuildServices.GetGuildData(Context.Guild.Id).GetUserData(_user.Id);
                var bitmap = GraphicsGenerator.GenerateUserCard(Context.Guild.Id, _user.Id);
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                memoryStream.Seek(0, SeekOrigin.Begin);
                bitmap.Dispose();

                await Context.Channel.SendFileAsync(memoryStream, "generated.png");
            }
        }

        [Command("card")]
        [RequireContext(ContextType.Guild)]
        public async Task DisplayUserCard(ulong _id)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {                
                var bitmap = GraphicsGenerator.GenerateUserCard(Context.Guild.Id, _id);
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                memoryStream.Seek(0, SeekOrigin.Begin);
                bitmap.Dispose();

                await Context.Channel.SendFileAsync(memoryStream, "generated.png");
            }
        }

        [Command("setCustomCardQuote")]
        [RequireContext(ContextType.Guild)]
        public async Task SetCustomCardQuote(string _quote)
        {
            if (InventoryItemServices.GetUserInventory(Context.Guild.Id, Context.User.Id).HasItem(1027))
            {
                InventoryItemServices.GetUserInventory(Context.Guild.Id, Context.User.Id).RemoveItem(1027);
                GuildServices.GetGuildData(Context.Guild.Id).GetUserData(Context.User.Id).miscData.SetCustomCardQuote(_quote);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    var bitmap = GraphicsGenerator.GenerateUserCard(Context.Guild.Id, Context.User.Id);
                    bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    bitmap.Dispose();

                    await Context.Channel.SendFileAsync(memoryStream, "generated.png", "Successfully applied your Custom Quote to your usercard!");
                }
            }
            else
            {
                await ReplyAsync($"Acquire a **Custom Card Quote** first for **2000** credits!");
            }
        }

        [Command("removeCustomCardQuote")]
        [RequireContext(ContextType.Guild)]
        public async Task RemoveCustomCardQuote()
        {
            GuildServices.GetGuildData(Context.Guild.Id).GetUserData(Context.User.Id).miscData.RemoveCustomCardQuote();
            await ReplyAsync($"Successfully removed your Custom Quote from your usercard!");
        }

        [Command("calculateRequiredXP")]
        [RequireContext(ContextType.Guild)]
        public async Task CalculateReqXP(int _next, int _now = -1)
        {
            var rank = GuildServices.GetGuildData(Context.Guild.Id).GetUserData(Context.User.Id).rank;
            var requiredXP = 0;
            if (_now == -1)
            {
                requiredXP = CalcReqXP(_next) - CalcReqXP(rank.currentRank);
                _now = rank.currentRank - 1;
            }
            else if (_now == 0)
            {
                requiredXP = CalcReqXP(_next);
            }
            else
            {
                requiredXP = CalcReqXP(_next) - CalcReqXP(_now);
            }

            await ReplyAsync($"You will need {requiredXP}xp to jump from Rank {_now + 1} to Rank {_next}");
        }

        private static int CalcReqXP(int _rank)
        {
            return (int)(1000 * ((Math.Pow(_rank, 2) + _rank) / 2));
        }

        [Command("leaderboard")]
        public async Task GetGuildLeaderboard()
        {
            var list = GuildServices.GetGuildData(Context.Guild.Id).userDatas;
            var sorted = list.OrderBy(o => o.Value.rank.GetRankIndex()).ToList();
            sorted.Reverse();
            if (sorted.Count > 10)
            {
                sorted.RemoveRange(10, sorted.Count - 10);
            }

            var embed = new EmbedBuilder();
            embed.WithTitle($"User rankings in {Context.Guild.Name}!");
            embed.WithColor(Color.Green);

            foreach (var kUser in sorted)
            {
                var user = kUser.Value;

                embed.AddField($"{UserCacheServices.GetUserCache(user.userID).userName}", $"Rank: {user.rank.currentRank} with rank index {user.rank.GetRankIndex()}");
            }

            await ReplyAsync(embed: embed.Build());
        }

        [Command("enumerate.guildData(GUILD_CURRENT).AllUsers().sortBy(UserRank.RankIndex(0))")]
        [RequireUserPermission(ChannelPermission.ManageChannels)]
        [RequireContext(ContextType.Guild)]
        public async Task Enumerateusers()
        {
            var sorted = Services.GuildServices.GetGuildData(Context.Guild.Id).userDatas.OrderBy(o => o.Value.rank.GetRankIndex()).ToList();

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("SORTED RANK");
            foreach (var user in sorted)
            {
                stringBuilder.AppendLine($"USER {user.Value.userID} at Rank {user.Value.rank.currentRank} with RANK INDEX {user.Value.rank.GetRankIndex()}");
            }
            await ReplyAsync(stringBuilder.ToString());
        }
    }
}
