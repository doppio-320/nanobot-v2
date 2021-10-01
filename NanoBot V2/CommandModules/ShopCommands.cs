using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBot_V2.CommandModules
{
    public class ShopCommands : ModuleBase<SocketCommandContext>
    {
        [Command("buy")]        
        public async Task Buy(int _id, int _amount = 1)
        {
            var item = Services.InventoryItemServices.FromID(_id);
            var prevMoney = Services.GuildServices.GetGuildData(Context.Guild.Id).GetUserData(Context.User.Id).balance.balance;
            if (Services.StoreServices.Buy(Context.Guild.Id, Context.User.Id, _id, _amount))
            {
                await ReplyAsync($"Successfully purchased **{_amount}** **{item.itemName}(s)** {item.itemIcon} for " +
                    $"{prevMoney - Services.GuildServices.GetGuildData(Context.Guild.Id).GetUserData(Context.User.Id).balance.balance}");
            }
            else
            {
                await ReplyAsync("Purchase has failed");
            }
        }

        [Command("buy")]
        public async Task Buy(string _alntID, int _amount = 1)
        {
            var item = Services.InventoryItemServices.FromAltnID(_alntID);

            if (item == null)
            {
                item = Services.InventoryItemServices.FromName(_alntID);
            }

            var prevMoney = Services.GuildServices.GetGuildData(Context.Guild.Id).GetUserData(Context.User.Id).balance.balance;
            if (Services.StoreServices.Buy(Context.Guild.Id, Context.User.Id, item.itemID, _amount))
            {
                await ReplyAsync($"Successfully purchased **{_amount}** **{item.itemName}(s)** {item.itemIcon} for " +
                    $"{prevMoney - Services.GuildServices.GetGuildData(Context.Guild.Id).GetUserData(Context.User.Id).balance.balance}");
            }
            else
            {
                await ReplyAsync("Purchase has failed");
            }
        }

        [Command("sell")]
        public async Task Sell(int _id, int _amount = 1)
        {
            var item = Services.InventoryItemServices.FromID(_id);
            var prevMoney = Services.GuildServices.GetGuildData(Context.Guild.Id).GetUserData(Context.User.Id).balance.balance;
            if (Services.StoreServices.Sell(Context.Guild.Id, Context.User.Id, _id, _amount))
            {                
                await ReplyAsync($"Successfully sold **{_amount}** **{item.itemName}(s)** {item.itemIcon} for" +
                    $" {Services.GuildServices.GetGuildData(Context.Guild.Id).GetUserData(Context.User.Id).balance.balance - prevMoney}");
            }
            else
            {
                await ReplyAsync("Transaction has failed");
            }
        }

        [Command("sell")]
        public async Task Sell(string _alntID, int _amount = 1)
        {
            var item = Services.InventoryItemServices.FromAltnID(_alntID);

            if (item == null)
            {
                item = Services.InventoryItemServices.FromName(_alntID);
            }

            var prevMoney = Services.GuildServices.GetGuildData(Context.Guild.Id).GetUserData(Context.User.Id).balance.balance;
            if (Services.StoreServices.Sell(Context.Guild.Id, Context.User.Id, item.itemID, _amount))
            {
                await ReplyAsync($"Successfully sold **{_amount}** **{item.itemName}(s)** {item.itemIcon} for" +
                    $" {Services.GuildServices.GetGuildData(Context.Guild.Id).GetUserData(Context.User.Id).balance.balance - prevMoney}");
            }
            else
            {
                await ReplyAsync("Transaction has failed");
            }
        }

        [Command("catalog product")]
        public async Task ShowProduct(int _id)
        {
            var item = Services.InventoryItemServices.FromID(_id);

            await ReplyAsync($"**{item.itemName}** {item.itemIcon} [ID: {item.itemID}, *{item.altnID}*]\n" +
                $"Buy for: {Services.StoreServices.BuyPrice(_id)}, Sell for: {Services.StoreServices.SellPrice(_id)}");
        }

        [Command("catalog product")]
        public async Task ShowProduct(string _altn)
        {
            var item = Services.InventoryItemServices.FromAltnID(_altn);

            if (item == null)
            {
                item = Services.InventoryItemServices.FromName(_altn);
            }

            await ReplyAsync($"**{item.itemName}** {item.itemIcon} [ID: {item.itemID}, *{item.altnID}*]\n" +
                $"Buy for: {Services.StoreServices.BuyPrice(item.itemID)}, Sell for: {Services.StoreServices.SellPrice(item.itemID)}");
        }
    }
}
