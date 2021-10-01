using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NanoBot_V2.CommandModules
{
    public class AutoresCommands : ModuleBase<SocketCommandContext>
    {
        [Command("addautores")]
        [Alias("createautores")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task AddAutores(string _condition, string _sensitivity, string _trigger, string _response)
        {
            Services.AutoresCondition triggerMode = Services.AutoresResponse.ParseAutoresCondition(_condition);

            bool isCaseSensitive = false;
            try
            {
                isCaseSensitive = bool.Parse(_sensitivity);
            }
            catch (Exception e) { }

            var result = Services.AutoresService.AddAutoRes(Context, _trigger, _response, triggerMode, isCaseSensitive);

            if (result)
            {
                await ReplyAsync($"Successfully created autores!\n" +
                    $"Trigger: **{_trigger}**\n" +
                    $"Response: **{_response}**\n" +
                    $"Condition: **AutoresConditon.{triggerMode}**\n" +
                    $"Is case sensitive: **{isCaseSensitive}**");
                await Context.Message.AddReactionAsync(new Emoji("✅"));
            }
            else
            {
                await ReplyAsync($"Unknown error code 7376: Autores already exists");
            }
        }

        [Command("listautores")]
        [Alias("showautores")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task ListAutores()
        {
            var autoress = Services.AutoresService.GetAutoRess(Context);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Autoresponses for {Context.Guild.Name}\n");            
            for (int i = 0; i < autoress.Count; i++)
            {
                sb.AppendLine($"ID: **{i}**\n" +
                    $"Trigger: **{autoress[i].trigger}**\n" +
                    $"Response: **{autoress[i].response}**\n" +
                    $"Condition: **{autoress[i].condition}**\n" +
                    $"Is case sensitive: **{autoress[i].caseSensitive}**\n");                
            }

            await ReplyAsync(sb.ToString());
        }

        [Command("deleteautores")]
        [Alias("removeautores")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task DeleteAutores(int _id)
        {
            var result = Services.AutoresService.RemoveAutores(Context, _id);

            if (result)
            {
                await ReplyAsync($"Removed autores");
                await Context.Message.AddReactionAsync(new Emoji("🗑"));
            }
            else
            {
                await ReplyAsync($"Autores does not exist");                
            }
        }
    }
}
