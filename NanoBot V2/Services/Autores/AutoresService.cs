using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBot_V2.Services
{
    public static class AutoresService
    {
        public static bool MessageCouldTriggerAutores(SocketCommandContext _ctx)
        {
            return GuildServices.GetGuildData(_ctx.Guild.Id).autoresData.ResponsePossible(_ctx.Message.Content);
        }

        public static string GetAutores(SocketCommandContext _ctx)
        {
            return GuildServices.GetGuildData(_ctx.Guild.Id).autoresData.ReturnAutores(_ctx.Message.Content);
        }

        public static bool AddAutoRes(SocketCommandContext _ctx, string _trig, string _res, AutoresCondition _cond = AutoresCondition.Contains, bool _case = false)
        {
            return GuildServices.GetGuildData(_ctx.Guild.Id).autoresData.AddAutores(_trig, _res, _cond, _case);
        }

        public static List<AutoresResponse> GetAutoRess(SocketCommandContext _ctx)
        {
            return GuildServices.GetGuildData(_ctx.Guild.Id).autoresData.GetAutoresResponses();
        }

        public static bool RemoveAutores(SocketCommandContext _ctx, int _id)
        {
            return GuildServices.GetGuildData(_ctx.Guild.Id).autoresData.RemoveAutores(_id);
        }
    }
}
