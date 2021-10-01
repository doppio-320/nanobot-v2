using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBot_V2.CommandModules
{
    public class DoppieCommands : ModuleBase<SocketCommandContext>
    {
        [Command("forceSave GUILDDATA")]
        [RequireContext(ContextType.Guild)]
        public async Task BackupGuildData(bool _bkp)
        {
            if (Context.User.Id != 252295989568733185)
                return;

            Services.GuildServices.Save(_bkp);
        }

        [Command("forceSave CACHEDATA")]
        [RequireContext(ContextType.Guild)]
        public async Task BackupCacheData(bool _bkp)
        {
            if (Context.User.Id != 252295989568733185)
                return;

            Services.UserCacheServices.Save(_bkp);
        }
    }
}
