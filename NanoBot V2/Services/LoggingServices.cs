using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBot_V2.Services
{
    public static class LoggingServices
    {
        public static void LogChannelSet(SocketCommandContext _ctx)
        {            
            var chnl = _ctx.Guild.GetChannel(GuildServices.GetGuildData(_ctx.Guild.Id).logChannelID) as IMessageChannel;

            var embed = new EmbedBuilder();
            embed.WithTitle($"LOGS ENABLED!");
            embed.WithColor(Color.Green);            
            embed.AddField($"Time", DateTime.Now);
            chnl.SendMessageAsync(embed: embed.Build());
        }

        public static void LogMemberKicked(SocketCommandContext _ctx, IGuildUser _user, string _reason)
        {            
            var chnl = _ctx.Guild.GetChannel(GuildServices.GetGuildData(_ctx.Guild.Id).logChannelID) as IMessageChannel;

            var embed = new EmbedBuilder();
            embed.WithTitle($"USER KICKED!");
            embed.WithColor(Color.Red);
            if(_reason != null)
            {
                embed.AddField($"Reason", _reason);
            }            
            embed.AddField($"Offender", _user.Username);
            embed.AddField($"Staff", _ctx.User);
            embed.AddField($"Time", DateTime.Now);
            chnl.SendMessageAsync(embed: embed.Build());
        }

        public static void LogMemberBanned(SocketCommandContext _ctx, IGuildUser _user, string _reason, int _duration)
        {            
            var chnl = _ctx.Guild.GetChannel(GuildServices.GetGuildData(_ctx.Guild.Id).logChannelID) as IMessageChannel;

            var embed = new EmbedBuilder();
            embed.WithTitle($"USER BANNED!");
            embed.WithColor(Color.Red);
            if (_reason != null)
            {
                embed.AddField($"Reason", _reason);
            }
            if(_duration == 0)
            {
                embed.AddField($"Duration", _duration);
            }
            embed.AddField($"Offender", _user.Username);
            embed.AddField($"Staff", _ctx.User);
            embed.AddField($"Time", DateTime.Now);
            chnl.SendMessageAsync(embed: embed.Build());
        }

        public static void LogMemberMuted(SocketCommandContext _ctx, IGuildUser _user, string _reason, int _duration)
        {            
            var chnl = _ctx.Guild.GetChannel(GuildServices.GetGuildData(_ctx.Guild.Id).logChannelID) as IMessageChannel;

            var embed = new EmbedBuilder();
            embed.WithTitle($"USER MUTED!");
            embed.WithColor(Color.Orange);
            if (_reason != null)
            {
                embed.AddField($"Reason", _reason);
            }
            if (_duration == 0)
            {
                embed.AddField($"Duration", _duration);
            }
            embed.AddField($"Offender", _user.Username);
            embed.AddField($"Staff", _ctx.User);
            embed.AddField($"Time", DateTime.Now);
            chnl.SendMessageAsync(embed: embed.Build());
        }

        public static void LogMessageDeleted(IMessage _msg)
        {            
            var srcChannel = (SocketGuildChannel)_msg.Channel;
            var guild = srcChannel.Guild;
            var chnl = guild.GetChannel(GuildServices.GetGuildData(guild.Id).logChannelID) as IMessageChannel;

            if (chnl == null)
                return;

            var embed = new EmbedBuilder();
            embed.WithTitle($"MESSAGE DELETED!");            
            embed.AddField($"USER", _msg.Author.Mention);
            try
            {
                embed.AddField($"CONTENTS", _msg.Content);
            }            catch (Exception e)
            {
                embed.AddField($"CONTENTS FAILED TO RETRIEVE", "NULL");
            }
            embed.AddField($"AT", _msg.Channel.Name);
            embed.AddField($"Time", DateTime.Now);
            chnl.SendMessageAsync(embed: embed.Build());
        }

        public static void LogMessageEdited(IMessage _msg, IMessage _newMsg)
        {
            var srcChannel = (SocketGuildChannel)_msg.Channel;
            var guild = srcChannel.Guild;
            var chnl = guild.GetChannel(GuildServices.GetGuildData(guild.Id).logChannelID) as IMessageChannel;

            if (chnl == null)
                return;

            var embed = new EmbedBuilder();
            embed.WithTitle($"MESSAGE EDITED!");
            embed.AddField($"USER", _msg.Author.Mention);
            embed.AddField($"BEFORE", _msg.Content);
            embed.AddField($"AFTER", _newMsg.Content);
            embed.AddField($"AT", _msg.Channel.Name);
            embed.AddField($"Time", DateTime.Now);
            chnl.SendMessageAsync(embed: embed.Build());
        }
    }
}
