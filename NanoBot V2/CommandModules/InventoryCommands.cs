using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NanoBot_V2.CommandModules
{
    public class InventoryCommands : ModuleBase<SocketCommandContext>
    {
        [Command("inventory")]
        [Alias("inv")]
        public async Task Snipe()
        {
            var inventory = Services.InventoryItemServices.GetUserInventory(Context.Guild.Id, Context.User.Id).storedItems;

            var embed = new EmbedBuilder();
            embed.WithTitle($"{Context.User.Id}'s backpack!");
            embed.WithColor(Color.Magenta);            
            foreach (var item in inventory)
            {
                embed.AddField($"{Services.InventoryItemServices.FromID(item.Key).itemName} {Services.InventoryItemServices.FromID(item.Key).itemIcon}", $"x{item.Value}");
            }            
            await ReplyAsync(embed: embed.Build());
        }

        [Command("inv_debugAdd")]
        [Alias("inv")]        
        public async Task Add(int _id, int _amount = 1)
        {
            Services.InventoryItemServices.GetUserInventory(Context.Guild.Id, Context.User.Id).AddItem(_id, _amount);
        }

        [Command("inv_debugRemove")]
        [Alias("inv")]
        public async Task Remove(int _id, int _amount = 1)
        {
            Services.InventoryItemServices.GetUserInventory(Context.Guild.Id, Context.User.Id).RemoveItem(_id, _amount);
        }
    }
}
