using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;
using Discord;

namespace NanoBot_V2.CommandModules
{
    public class ModerationCommands : ModuleBase<SocketCommandContext>
    {
        [Command("kick")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task ModKick(IGuildUser _user, [Remainder] string _reason = null)
        {
            await _user.KickAsync(_reason);
            await Context.Message.AddReactionAsync(new Emoji("👋"));
            if (_reason == null)
                await ReplyAsync($"Kicked {_user.Username}");
            else
                await ReplyAsync($"Kicked {_user.Username}, reason: {_reason}");
            Services.LoggingServices.LogMemberKicked(Context, _user, _reason);
        }

        [Command("ban")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task ModBan(IGuildUser _user, string _reason = null, int _duration = 0)
        {
            await _user.BanAsync(0, _reason);
            await Context.Message.AddReactionAsync(new Emoji("🔨"));

            if (_reason == null)
                await ReplyAsync($"Banned {_user.Username}");
            else if (_duration == 0)
                await ReplyAsync($"Banned {_user.Username}, reason: {_reason}");
            else
                await ReplyAsync($"Banned {_user.Username} for {_duration} day(s), reason: {_reason}");
            Services.LoggingServices.LogMemberBanned(Context, _user, _reason, _duration);
        }

        [Command("mute")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task ModMute(IGuildUser _user, string _reason = null, int _duration = 0)
        {
            if (Services.GuildServices.GetGuildData(Context.Guild.Id).muteRoleID == 0)
            {
                await ReplyAsync($"Mute role not set! Set mute role now using ``n2!configureMuteRole @<muted role>``");
                return;
            }

            var muteRole = Context.Guild.GetRole(Services.GuildServices.GetGuildData(Context.Guild.Id).muteRoleID);
            if (Context.Guild.GetUser(_user.Id).Roles.Any(r => r == muteRole))
            {
                await Context.Message.AddReactionAsync(new Emoji("✅"));
                await _user.RemoveRoleAsync(muteRole);

                await ReplyAsync($"Unmuted {_user.Username}");
            }
            else
            {
                await _user.AddRoleAsync(muteRole);
                await Context.Message.AddReactionAsync(new Emoji("🤫"));

                if (_reason == null)
                    await ReplyAsync($"Muted {_user.Username}");
                else if (_duration == 0)
                    await ReplyAsync($"Muted {_user.Username}, reason: {_reason}");
                else
                    await ReplyAsync($"Muted {_user.Username} for {_duration} day(s), reason: {_reason}");
                Services.LoggingServices.LogMemberMuted(Context, _user, _reason, _duration);
            }
        }

        [Command("unmute")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task ModUnMute(IGuildUser _user, string _reason = null, int _duration = 0)
        {
            if (Services.GuildServices.GetGuildData(Context.Guild.Id).muteRoleID == 0)
            {
                await ReplyAsync($"Mute role not set! Set mute role now using ``n2!configureMuteRole @<muted role>``");
                return;
            }

            var muteRole = Context.Guild.GetRole(Services.GuildServices.GetGuildData(Context.Guild.Id).muteRoleID);
            if (Context.Guild.GetUser(_user.Id).Roles.Any(r => r == muteRole))
            {
                await Context.Message.AddReactionAsync(new Emoji("✅"));
                await _user.RemoveRoleAsync(muteRole);

                await ReplyAsync($"Unmuted {_user.Username}");
            }
        }

        [Command("echo")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task Echo(string _msg, IGuildChannel _channel = null)
        {
            if(_channel == null)
            {
                await ReplyAsync(_msg);
                await Context.Message.DeleteAsync();
            }
            else
            {
                await Context.Guild.GetTextChannel(_channel.Id).SendMessageAsync(_msg);
                await Context.Message.DeleteAsync();
            }
        }
    }
}
